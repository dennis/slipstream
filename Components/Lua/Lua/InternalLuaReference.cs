#nullable enable

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NLua;

using Serilog;

using Slipstream.Components.Internal;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.Lua.Lua
{
    public class InternalLuaReference : ILuaReference
    {
        private readonly IEventBus EventBus;
        private readonly IInternalEventFactory EventFactory;
        private readonly IEventEnvelope Envelope;
        private readonly IEventEnvelope BroadcastEnvelope;
        private readonly ILogger Logger;

        public string InstanceId { get; }
        public string LuaScriptInstanceId { get; }

        public InternalLuaReference(string instanceId, string luaScriptInstanceId, IEventBus eventBus, IInternalEventFactory eventFactory, ILogger logger)
        {
            InstanceId = instanceId;
            LuaScriptInstanceId = luaScriptInstanceId;
            EventBus = eventBus;
            EventFactory = eventFactory;
            Envelope = new EventEnvelope(luaScriptInstanceId).Add(instanceId);
            BroadcastEnvelope = new EventEnvelope(luaScriptInstanceId);
            Logger = logger;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void send_custom_event(string name, object payload)
        {
            string json;
            if (payload is LuaTable luaTable)
            {
                json = JsonConvert.SerializeObject(Parameters.From(luaTable));
            }
            else
            {
                json = payload as string ?? "";

                try
                {
                    JObject.Parse(json);
                }
                catch (Newtonsoft.Json.JsonReaderException)
                {
                    // TODO: It would be really useful to get linenumber here
                    Logger.Error($"{LuaScriptInstanceId}: send_custom_event(): JSON Invalid: {json}");
                    return;
                }
            }

            EventBus.PublishEvent(EventFactory.CreateInternalCustomEvent(BroadcastEnvelope, name, json));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void shutdown()
        {
            EventBus.PublishEvent(EventFactory.CreateInternalCommandShutdown(Envelope));
        }
    }
}