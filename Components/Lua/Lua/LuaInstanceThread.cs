#nullable enable

using NLua;
using Serilog;
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
        private readonly Object Lock = new object();
        private readonly NLua.Lua Lua = new NLua.Lua();
        private static readonly Random random = new Random();
        private readonly ILuaLibraryRepository Repository;
        private readonly IEventBusSubscription Subscription;
        private readonly IEventHandlerController EventHandlerController;
        private readonly LuaLuaLibrary LuaLibrary;
        private readonly List<ILuaReference> CreatedReferences = new List<ILuaReference>();
        private readonly IDictionary<string, DelayedExecution> DebounceDelayedFunctions = new Dictionary<string, DelayedExecution>();
        private readonly IDictionary<string, DelayedExecution> WaitDelayedFunctions = new Dictionary<string, DelayedExecution>();
        private readonly string FileName = "";
        private ulong LastLuaGC;

        public LuaInstanceThread(string instanceId, string filePath, LuaLuaLibrary luaLibrary, ILogger logger, ILuaLibraryRepository repository, IEventBusSubscription subscription, IEventHandlerController eventHandlerController) : base(instanceId, logger)
        {
            FileName = filePath;
            Repository = repository;
            Subscription = subscription;
            EventHandlerController = eventHandlerController;
            LuaLibrary = luaLibrary;

            SetupLua(Lua);
        }

        protected override void Main()
        {
            LuaFunction? handleFunc = null;

            try
            {
                lock (Lock)
                {
                    // Fix paths, so we can require() files relative to where the script is located
                    var scriptPath = Path.GetDirectoryName(FileName).Replace("\\", "\\\\");
                    Lua.DoString($"package.path = \"{scriptPath}\\\\?.lua;\" .. package.path;");

                    Lua.DoFile(FileName);

                    handleFunc = Lua["handle"] as NLua.LuaFunction;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "{fileName} errored: {message}", FileName, e.Message);
            }

            // If we have no HandleFunc defined, then just exit as this script will never do anything
            if (handleFunc == null && WaitDelayedFunctions.Count == 0 && DebounceDelayedFunctions.Count == 0)
            {
                Logger.Warning("{fileName} got no handle(), no wait() nor debounce() functions. Stopping", FileName);
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
                                if (@event.Uptime - LastLuaGC > 1000)
                                {
                                    LastLuaGC = @event.Uptime;
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

            Debug.WriteLine($"[{FileName}] Stopping. Clearning up {CreatedReferences.Count} references");
            CreatedReferences.Clear();

            LuaLibrary.InstanceStopped(InstanceId);
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

        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public ILuaLibrary? require(string name)
        {
            lock (Lock)
            {
                var i = Repository.Get(name);
                if (i == null)
                    return null;
                return new LuaLibraryInstanceTracker(i, this);
            }
        }

        private void ReferenceCreated(ILuaReference inst)
        {
            lock (Lock)
            {
                CreatedReferences.Add(inst);
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

        private void HandleDelayedExecution(IDictionary<string, DelayedExecution> functions)
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
    }
}