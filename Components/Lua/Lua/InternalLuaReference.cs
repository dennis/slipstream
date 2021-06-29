#nullable enable

using Slipstream.Components.Internal;
using Slipstream.Shared;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.Lua.Lua
{
    public class InternalLuaReference : ILuaReference
    {
        private readonly IEventBus EventBus;
        private readonly IInternalEventFactory EventFactory;
        private readonly IEventEnvelope Envelope;

        public string InstanceId { get; }
        public string LuaScriptInstanceId { get; }

        public InternalLuaReference(string instanceId, string luaScriptInstanceId, IEventBus eventBus, IInternalEventFactory eventFactory)
        {
            InstanceId = instanceId;
            LuaScriptInstanceId = luaScriptInstanceId;
            EventBus = eventBus;
            EventFactory = eventFactory;
            Envelope = new EventEnvelope(luaScriptInstanceId).Add(instanceId);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void shutdown()
        {
            EventBus.PublishEvent(EventFactory.CreateInternalCommandShutdown(Envelope));
        }
    }
}