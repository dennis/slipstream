using Slipstream.Shared;

namespace Slipstream.Components.FileMonitor.Lua
{
    public class FileMonitorLuaReference : IFileMonitorLuaReference
    {
        private readonly string InstanceId;
        private readonly IEventBus EventBus;
        private readonly IFileMonitorEventFactory EventFactory;

        public FileMonitorLuaReference(string instanceId, IEventBus eventBus, IFileMonitorEventFactory eventFactory)
        {
            InstanceId = instanceId;
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void scan()
        {
            EventBus.PublishEvent(EventFactory.CreateFileMonitorCommandScan(InstanceId));
        }

        public void Dispose()
        {
        }
    }
}