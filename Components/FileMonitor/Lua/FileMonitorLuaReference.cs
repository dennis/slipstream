using Slipstream.Shared;

namespace Slipstream.Components.FileMonitor.Lua
{
    public class FileMonitorLuaReference : IFileMonitorLuaReference
    {
        private readonly IEventBus EventBus;
        private readonly IFileMonitorEventFactory EventFactory;
        private readonly FileMonitorLuaLibrary LuaLibrary;
        public string InstanceId { get; }

        public FileMonitorLuaReference(string instanceId, FileMonitorLuaLibrary luaLibrary, IEventBus eventBus, IFileMonitorEventFactory eventFactory)
        {
            InstanceId = instanceId;
            EventBus = eventBus;
            EventFactory = eventFactory;
            LuaLibrary = luaLibrary;
        }

        public void scan()
        {
            EventBus.PublishEvent(EventFactory.CreateFileMonitorCommandScan(InstanceId));
        }

        public void Dispose()
        {
            LuaLibrary.ReferenceDropped(this);
        }
    }
}