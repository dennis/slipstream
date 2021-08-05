#nullable enable

using NLua;
using NLua.Exceptions;

using Serilog;

using Slipstream.Components.Internal;
using Slipstream.Shared;
using Slipstream.Shared.Lua;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Slipstream.Components.Lua.Lua
{
    public partial class LuaInstanceThread : BaseInstanceThread, ILuaInstanceThread
    {
        private class Dependency
        {
            public string InstanceId { get; }
            public string LuaScriptInstanceId { get; }

            public Dependency(string instanceId, string luaScriptInstanceId)
            {
                InstanceId = instanceId;
                LuaScriptInstanceId = luaScriptInstanceId;
            }
        }

        private readonly Object Lock = new object();
        private static readonly Random random = new Random();
        private readonly ILuaLibraryRepository Repository;
        private readonly IEventBusSubscription Subscription;
        private readonly IEventHandlerController EventHandlerController;
        private readonly LuaLuaLibrary LuaLibrary;
        private readonly IDictionary<string, DelayedExecution> DebounceDelayedFunctions = new Dictionary<string, DelayedExecution>();
        private readonly IDictionary<string, DelayedExecution> WaitDelayedFunctions = new Dictionary<string, DelayedExecution>();
        private readonly string FileName = "";
        private readonly List<Dependency> Dependencies = new List<Dependency>();
        private readonly IEventEnvelope Envelope;
        private NLua.Lua Lua = new NLua.Lua();
        private ulong LastLuaGC;

        public LuaInstanceThread(
            string luaLibraryName,
            string instanceId,
            string filePath,
            LuaLuaLibrary luaLibrary,
            ILogger logger,
            ILuaLibraryRepository repository,
            IEventBusSubscription subscription,
            IEventHandlerController eventHandlerController,
            IEventBus eventBus,
            IInternalEventFactory internalEventFactory) : base(luaLibraryName, instanceId, logger, eventHandlerController, eventBus, internalEventFactory)
        {
            FileName = filePath;
            Repository = repository;
            Subscription = subscription;
            EventHandlerController = eventHandlerController;
            LuaLibrary = luaLibrary;
            EventBus = eventBus;
            InternalEventFactory = internalEventFactory;
            Envelope = new EventEnvelope(instanceId);
        }

        protected override void Main()
        {
            try
            {
                var handleFunc = LoadFile();

                // If we have no HandleFunc defined, then just exit as this script will never do anything
                if (handleFunc == null && WaitDelayedFunctions.Count == 0 && DebounceDelayedFunctions.Count == 0)
                {
                    Logger.Warning("{fileName} got no handle(), wait() nor debounce() functions. Stopping", FileName);
                }
                else
                {
                    while (!Stopping)
                    {
                        var @event = Subscription.NextEvent(100);

                        if (@event != null)
                        {
                            EventHandlerController.HandleEvent(@event);

                            try
                            {
                                lock (Lock)
                                {
                                    handleFunc?.Call(@event);

                                    // Perform GC in Lua approx every second
                                    if (@event.Envelope.Uptime - LastLuaGC > 1000)
                                    {
                                        LastLuaGC = @event.Envelope.Uptime;
                                        Lua.DoString("collectgarbage()");
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Logger.Error(e, "{fileName} errored while invoking handle(): {message}", FileName, e.Message);
                            }
                        }

                        HandleDelayedExecution(WaitDelayedFunctions);
                        HandleDelayedExecution(DebounceDelayedFunctions);
                    }
                }
            }
            catch (LuaScriptException e)
            {
                string message = e.InnerException?.Message ?? e.Message;

                Logger.Error(e, "{source} errored: {message}", e.Source, message);
            }
            catch (Exception e)
            {
                Logger.Error(e, "{fileName} errored: {message}", FileName, e.Message);
            }

            LuaLibrary.InstanceStopped(InstanceId);

            foreach (var dependency in Dependencies)
            {
                EventBus.PublishEvent(InternalEventFactory.CreateInternalDependencyRemoved(Envelope, LuaLibraryName, dependency.LuaScriptInstanceId, dependency.InstanceId));
            }

            Dependencies.Clear();
        }

        private LuaFunction? LoadFile()
        {
            lock (Lock)
            {
                Lua = new NLua.Lua();
                SetupLua(Lua);

                // Fix paths, so we can require() files relative to where the script is located
                var scriptPath = Path.GetDirectoryName(FileName)?.Replace("\\", "\\\\") ?? String.Empty;
                if (scriptPath != String.Empty)
                {
                    Lua.DoString($"package.path = \"{scriptPath}\\\\?.lua;\" .. package.path;");
                }

                Lua.DoFile(FileName);

                return Lua["handle"] as NLua.LuaFunction;
            }
        }

        private void SetupLua(NLua.Lua lua)
        {
            var hiddenRequireName = $"require_{RandomString(5)}"; // TODO: is there a better way?
            var hiddenSelf = $"slipstream_{RandomString(5)}";

            lua[hiddenSelf] = this;
            lua.DoString(@$"
local {hiddenRequireName} = require;

function require(n)
    local m = {hiddenSelf}:require(n)

    if not m then
      m = {hiddenRequireName}(n)
    end

    return m
end

function debounce(a, b, c); {hiddenSelf}:debounce(a, b, c); end
function wait(a, b, c); {hiddenSelf}:wait(a, b, c); end

-- internal slipstream stuff
SS = {{
    instance_id = ""{InstanceId.Replace("\\", "\\\\")}"",
    file = ""{FileName.Replace("\\", "\\\\")}""
}}
");
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public LuaLibraryInstanceTracker? require(string name)
        {
            lock (Lock)
            {
                var i = Repository.Get(name);
                if (i == null)
                    return null;
                return new LuaLibraryInstanceTracker(i, this, InstanceId);
            }
        }

        private void ReferenceCreated(ILuaReference inst)
        {
            lock (Lock)
            {
                if (inst == null)
                    return;

                var dependency = new Dependency(inst.InstanceId, inst.LuaScriptInstanceId);

                if (!Dependencies.Contains(dependency))
                {
                    Debug.WriteLine($"[{InstanceId}] depends on {inst.InstanceId}");

                    Dependencies.Add(dependency);

                    EventBus.PublishEvent(InternalEventFactory.CreateInternalDependencyAdded(Envelope, LuaLibraryName, InstanceId, dependency.InstanceId));
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void debounce(string name, LuaFunction func, float debounceLength)
        {
            if (func != null)
            {
                lock (Lock)
                    DebounceDelayedFunctions[name] = new DelayedExecution(func, DateTime.Now.AddSeconds(debounceLength));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void wait(string name, LuaFunction func, float duration)
        {
            if (func != null)
            {
                lock (Lock)
                {
                    if (!WaitDelayedFunctions.ContainsKey(name))
                        WaitDelayedFunctions[name] = new DelayedExecution(func, DateTime.Now.AddSeconds(duration));
                }
            }
        }

        private static void HandleDelayedExecution(IDictionary<string, DelayedExecution> functions)
        {
            var triggeredFunctions = new Dictionary<string, DelayedExecution>();

            foreach (var d in functions)
            {
                if (d.Value.TriggersAt < DateTime.Now)
                {
                    triggeredFunctions.Add(d.Key, d.Value);
                }
            }

            foreach (var d in triggeredFunctions)
            {
                functions.Remove(d.Key);
                d.Value.Function.Call();
            }
        }

        public new void Dispose()
        {
            base.Dispose();
        }
    }
}