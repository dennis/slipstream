using Slipstream.Shared;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.AppilcationUpdate.Lua
{
    public class ApplicationUpdateReference : BaseLuaReference, IApplicationUpdateReference
    {
        private ApplicationUpdateLuaLibrary LuaLibrary { get; }
        private readonly IApplicationUpdateEventFactory ApplicationUpdateEventFactory;
        private readonly IEventBus EventBus;

        public ApplicationUpdateReference(ApplicationUpdateLuaLibrary luaLibrary, string instanceId, string luaScriptInstanceId, IEventBus eventBus, IApplicationUpdateEventFactory eventFactory) : base(instanceId, luaScriptInstanceId)
        {
            LuaLibrary = luaLibrary;
            ApplicationUpdateEventFactory = eventFactory;
            EventBus = eventBus;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void start()
        {
            EventBus.PublishEvent(ApplicationUpdateEventFactory.CreateApplicationUpdateCommandCheckLatestVersion(Envelope));
        }

        public override void Dispose()
        {
            LuaLibrary.ReferenceDropped(this);
        }
    }
}