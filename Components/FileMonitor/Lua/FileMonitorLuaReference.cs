using Slipstream.Shared;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.FileMonitor.Lua
{
    public class FileMonitorLuaReference : BaseLuaReference, IFileMonitorLuaReference
    {
        private readonly IEventBus EventBus;
        private readonly IFileMonitorEventFactory EventFactory;
        private readonly FileMonitorLuaLibrary LuaLibrary;

        public FileMonitorLuaReference(string instanceId, string luaScriptInstanceId, FileMonitorLuaLibrary luaLibrary, IEventBus eventBus, IFileMonitorEventFactory eventFactory) : base(instanceId, luaScriptInstanceId)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
            LuaLibrary = luaLibrary;
        }

        public void scan()
        {
            EventBus.PublishEvent(EventFactory.CreateFileMonitorCommandScan(Envelope));
        }

        public override void Dispose()
        {
            LuaLibrary.ReferenceDropped(this);
        }
    }
}