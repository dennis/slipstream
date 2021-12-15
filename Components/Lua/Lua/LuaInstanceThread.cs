#nullable enable

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NLua;
using NLua.Exceptions;

using Serilog;

using Slipstream.Components.Internal;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Lua;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

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

        private class ScriptCallbacksLuaFunc
        {
            // function that receives events
            public LuaFunction? HandleFunc { get; set; }

            // function that is invoked when script is stopped - "destructor"
            public LuaFunction? AtExitFunc { get; set; }
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
            var callbacks = new ScriptCallbacksLuaFunc();

            try
            {
                callbacks = LoadFile();

                // If we have no HandleFunc defined, then just exit as this script will never do anything
                if (callbacks.HandleFunc == null && WaitDelayedFunctions.Count == 0 && DebounceDelayedFunctions.Count == 0)
                {
                    Logger.Warning("{FileName} got no handle(), wait() nor debounce() functions. Stopping", FileName);
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
                                    callbacks.HandleFunc?.Call(@event);

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
                                Logger.Error(e, "{FileName} errored while invoking handle(): {Message}", FileName, e.Message);
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

                Logger.Error(e, "{Source} errored: {Message}", e.Source ?? FileName, message);
            }
            catch (Exception e)
            {
                Logger.Error(e, "{FileName} errored: {Message}", FileName, e.Message);
            }

            DebounceDelayedFunctions.Clear();
            WaitDelayedFunctions.Clear();

            callbacks.AtExitFunc?.Call();

            LuaLibrary.InstanceStopped(InstanceId);

            foreach (var dependency in Dependencies)
            {
                EventBus.PublishEvent(InternalEventFactory.CreateInternalDependencyRemoved(Envelope, LuaLibraryName, dependency.LuaScriptInstanceId, dependency.InstanceId));
            }

            Dependencies.Clear();
        }

        private ScriptCallbacksLuaFunc LoadFile()
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

                var handleFunc = Lua["handle"] as NLua.LuaFunction;
                var atExitFunc = Lua["atexit"] as NLua.LuaFunction;

                return new ScriptCallbacksLuaFunc { HandleFunc = handleFunc, AtExitFunc = atExitFunc };
            }
        }

        private void SetupLua(NLua.Lua lua)
        {
            var hiddenRequireName = $"require_{RandomString()}"; // TODO: is there a better way?
            var hiddenSelf = $"slipstream_{RandomString()}";

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
function parse_json(a); return {hiddenSelf}:parse_json(a); end
function generate_json(a); return {hiddenSelf}:generate_json(a); end

-- internal slipstream stuff
SS = {{
    instance_id = ""{InstanceId.Replace("\\", "\\\\")}"",
    file = ""{FileName.Replace("\\", "\\\\")}""
}}

SS.eventHandlers = {{}}
SS.eventHandlers.handlers = {{}}
SS.eventHandlers.add = function(self, eventName, func)
    if type(self) ~= ""table"" then
        error(""ERROR: Please use event:add(...) (note: colon, not comma)"")

        return
    end

    if not self.handlers[eventName] then
        self.handlers[eventName] = {{}}
    end

    table.insert(self.handlers[eventName], func)
end

SS.eventHandlers.handle = function(self, event)
    if type(self) ~= ""table"" then
        error(""ERROR: Please use event:handle(...) (note: colon, not comma)"")
        return
    end

    if self.handlers[event.EventType] then
        for _, h in ipairs(self.handlers[event.EventType]) do
            h(event)
        end
    end
end

function addEventHandler(eventName, func)
    SS.eventHandlers:add(eventName, func)

    if not handle then
        function handle(event)
            SS.eventHandlers:handle(event)
        end
    end
end
");
        }

        private static string RandomString()
        {
            const int length = 5;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return "__SS_" + new string(Enumerable.Repeat(chars, length)
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
                    Logger.Debug("[{InstanceId}] depends on {DependencyInstanceId}", InstanceId, inst.InstanceId);

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
                var triggerAt = DateTime.Now.AddSeconds(debounceLength);
                lock (Lock)
                    DebounceDelayedFunctions[name] = new DelayedExecution(func, triggerAt);
                Logger.Debug("{InstanceId} Setting debounce() with name '{WaitName}' until {TriggerDateTime}", InstanceId, name, triggerAt);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void wait(string name, LuaFunction func, float duration)
        {
            if (func != null)
            {
                if (!WaitDelayedFunctions.ContainsKey(name))
                {
                    var triggerAt = DateTime.Now.AddSeconds(duration);
                    lock (Lock)
                        WaitDelayedFunctions[name] = new DelayedExecution(func, triggerAt);
                    Logger.Debug("{InstanceId} Adding wait() with name '{WaitName}' until {TriggerDateTime}", InstanceId, name, triggerAt);
                }
                else
                {
                    Logger.Debug("{InstanceId} Adding wait() with name '{WaitName}' ignored. We're already waiting", InstanceId, name);
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public LuaTable? parse_json(string jsonString)
        {
            try
            {
                return ParseJObjectAsLuaTable(JObject.Parse(jsonString));
            }
            catch (Newtonsoft.Json.JsonReaderException)
            {
                Logger.Warning("{FileName}: parse_json(): JSON Invalid. Tried to parse: {json}", FileName, jsonString);
                return null;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public static string generate_json(LuaTable luaTable)
        {
            return JsonConvert.SerializeObject(Parameters.From(luaTable));
        }

        private LuaTable ParseJObjectAsLuaTable(JObject jobj)
        {
            LuaTable? luaTable = CreateEmptyLuaTable();

            foreach (var kv in jobj)
            {
                luaTable[kv.Key] = ParseJToken(kv.Value);
            }

            return luaTable;
        }

        private object? ParseJToken(JToken? value)
        {
            if (value == null || value.Type == JTokenType.Null)
                return null;

            switch (value.Type)
            {
                case JTokenType.None:
                    return null;

                case JTokenType.Object:
                    return ParseJObjectAsLuaTable((JObject)value);

                case JTokenType.Array:
                    return ParseJArrayAsLuaTable((JArray)value);

                case JTokenType.Constructor:
                case JTokenType.Property:
                case JTokenType.Comment:
                case JTokenType.Null:
                case JTokenType.Undefined:
                case JTokenType.Date:
                case JTokenType.Raw:
                case JTokenType.Bytes:
                case JTokenType.Guid:
                case JTokenType.Uri:
                case JTokenType.TimeSpan:
                    throw new Exception($"JToken not supported: {value.Type}. Please report this as a bug");

                case JTokenType.Integer:
                    return value.ToObject<long>();

                case JTokenType.Float:
                    return value.ToObject<float>();

                case JTokenType.String:
                    return value.ToObject<string>();

                case JTokenType.Boolean:
                    return value.ToObject<bool>();

                default:
                    throw new Exception($"JToken not supported: {value.Type}. Please report this as a bug");
            }
        }

        private object ParseJArrayAsLuaTable(JArray v)
        {
            LuaTable? luaTable = CreateEmptyLuaTable();

            int i = 0;
            foreach (var value in v)
            {
                var key = "" + i;

                luaTable[key] = ParseJToken(value);

                i += 1;
            }

            return luaTable;
        }

        private LuaTable CreateEmptyLuaTable()
        {
            var tmpName = RandomString();
            Lua.NewTable(tmpName);
            var luaTable = Lua[tmpName] as LuaTable;
            Lua[tmpName] = null;

            Debug.Assert(luaTable != null);

            return luaTable!;
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