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
        private readonly InternalLuaLibrary LuaLibrary;
        public string InstanceId { get; }

        public InternalLuaReference(InternalLuaLibrary luaLibrary, string instanceId, IEventBus eventBus, IInternalEventFactory eventFactory)
        {
            InstanceId = instanceId;
            EventBus = eventBus;
            EventFactory = eventFactory;
            LuaLibrary = luaLibrary;
        }

        public void Dispose()
        {
            LuaLibrary.ReferenceDropped(this);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void shutdown()
        {
            EventBus.PublishEvent(EventFactory.CreateInternalCommandShutdown());
        }
    }
}