using Slipstream.Shared;

namespace Slipstream.Components.AppilcationUpdate.Lua
{
    public class ApplicationUpdateReference : IApplicationUpdateReference
    {
        private ApplicationUpdateLuaLibrary LuaLibrary { get; }
        public string InstanceId { get; }

        private readonly IApplicationUpdateEventFactory ApplicationUpdateEventFactory;
        private readonly IEventBus EventBus;

        public ApplicationUpdateReference(ApplicationUpdateLuaLibrary luaLibrary, string instanceId, IEventBus eventBus, IApplicationUpdateEventFactory eventFactory)
        {
            LuaLibrary = luaLibrary;
            InstanceId = instanceId;
            ApplicationUpdateEventFactory = eventFactory;
            EventBus = eventBus;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void start()
        {
            EventBus.PublishEvent(ApplicationUpdateEventFactory.CreateApplicationUpdateCommandCheckLatestVersion());
        }

        public void Dispose()
        {
            LuaLibrary.ReferenceDropped(this);
        }
    }
}