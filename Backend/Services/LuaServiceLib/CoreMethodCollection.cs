using NLua;
using Slipstream.Shared;
using System;
using System.Collections.Generic;

#nullable enable

namespace Slipstream.Backend.Services.LuaServiceLib
{
    public partial class LuaContext
    {
        public class CoreMethodCollection
        {
            private class DelayedExecution
            {
                public LuaFunction Function;
                public DateTime TriggersAt;

                public DelayedExecution(LuaFunction luaFunction, DateTime triggersAt)
                {
                    Function = luaFunction;
                    TriggersAt = triggersAt;
                }
            }

            private readonly string Prefix;
            private readonly IEventFactory EventFactory;
            private readonly IEventBus EventBus;
            private readonly IEventSerdeService EventSerdeService;

            private readonly IDictionary<string, DelayedExecution> DebounceDelayedFunctions = new Dictionary<string, DelayedExecution>();
            private readonly IDictionary<string, DelayedExecution> WaitDelayedFunctions = new Dictionary<string, DelayedExecution>();

            public static CoreMethodCollection Register(IEventFactory eventFactory, IEventBus eventBus, string logPrefix, IEventSerdeService eventSerdeService, Lua lua)
            {
                var m = new CoreMethodCollection(eventFactory, eventBus, logPrefix, eventSerdeService);

                m.Register(lua);

                return m;
            }

            public CoreMethodCollection(IEventFactory eventFactory, IEventBus eventBus, string logPrefix, IEventSerdeService eventSerdeService)
            {
                Prefix = logPrefix;
                EventFactory = eventFactory;
                EventBus = eventBus;
                EventSerdeService = eventSerdeService;
            }

            public void Register(Lua lua)
            {
                lua["core"] = this;

                // Make old Lua script work as before
                lua.DoString(@"
function print(s); core:print(s); end
function debounce(a, b, c); core:debounce(a, b, c); end
function wait(a, b, c); core:wait(a, b, c); end
function event_to_json(a); return core:event_to_json(a); end
");
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
            public void print(string s)
            {
                EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole($"{Prefix}: {s}"));
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
            public void debounce(string name, LuaFunction func, float debounceLength)
            {
                if (func != null)
                {
                    DebounceDelayedFunctions[name] = new DelayedExecution(func, DateTime.Now.AddSeconds(debounceLength));
                }
                else
                {
                    print("Can't debounce without a function");
                }
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
            public void wait(string name, LuaFunction func, float duration)
            {
                if (func != null)
                {
                    if (!WaitDelayedFunctions.ContainsKey(name))
                        WaitDelayedFunctions[name] = new DelayedExecution(func, DateTime.Now.AddSeconds(duration));
                }
                else
                {
                    print("Can't wait without a function");
                }
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
            public string event_to_json(IEvent @event)
            {
                return EventSerdeService.Serialize(@event);
            }

            public void Loop()
            {
                HandleDelayedExecution(WaitDelayedFunctions);
                HandleDelayedExecution(DebounceDelayedFunctions);
            }

            private void HandleDelayedExecution(IDictionary<string, DelayedExecution> functions)
            {
                string? deleteKey = null;

                foreach (var d in functions)
                {
                    if (d.Value.TriggersAt < DateTime.Now)
                    {
                        d.Value.Function.Call();
                        deleteKey = d.Key;
                        break;
                    }
                }

                if (deleteKey != null)
                {
                    functions.Remove(deleteKey);
                }
            }
        }
    }
}
