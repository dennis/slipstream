#nullable enable

using NLua;
using Serilog;
using Slipstream.Components.Internal;
using Slipstream.Shared;
using System;
using System.Collections.Generic;
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
        private readonly IDictionary<string, DelayedExecution> DebounceDelayedFunctions = new Dictionary<string, DelayedExecution>();
        private readonly IDictionary<string, DelayedExecution> WaitDelayedFunctions = new Dictionary<string, DelayedExecution>();
        private readonly string FileName = "";
        private readonly LuaFunction? HandleFunc;
        private ulong LastLuaGC;

        public LuaInstanceThread(string instanceId, string filePath, ILogger logger, ILuaLibraryRepository repository, IEventBusSubscription subscription, IEventHandlerController eventHandlerController) : base(instanceId, logger)
        {
            FileName = filePath;
            Repository = repository;
            Subscription = subscription;
            EventHandlerController = eventHandlerController;

            SetupLua(Lua);

            try
            {
                lock (Lock)
                {
                    // Fix paths, so we can require() files relative to where the script is located
                    var scriptPath = Path.GetDirectoryName(filePath).Replace("\\", "\\\\");
                    Lua.DoString($"package.path = \"{scriptPath}\\\\?.lua;\" .. package.path;");

                    Lua.DoFile(filePath);

                    HandleFunc = Lua["handle"] as NLua.LuaFunction;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "{fileName} errored: {message}", FileName, e.Message);
            }
        }

        protected override void Main()
        {
            var internalEventHandler = EventHandlerController.Get<Internal.EventHandler.Internal>();
            internalEventHandler.OnInternalShutdown += (_, _e) => Stopping = true;

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
                            HandleFunc?.Call(@event);

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
");
        }

        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public ILuaLibrary require(string name)
        {
            lock (Lock)
            {
                return Repository.Get(name);
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

        public void Stop()
        {
            Stopping = true;
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