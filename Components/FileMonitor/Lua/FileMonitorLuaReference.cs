using Slipstream.Shared;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.FileMonitor.Lua
{
    public class FileMonitorLuaReference : BaseLuaReference, IFileMonitorLuaReference
    {
        private readonly IEventBus EventBus;
        private readonly IFileMonitorEventFactory EventFactory;

        public FileMonitorLuaReference(string instanceId, string luaScriptInstanceId, IEventBus eventBus, IFileMonitorEventFactory eventFactory) : base(instanceId, luaScriptInstanceId)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void scan()
        {
            EventBus.PublishEvent(EventFactory.CreateFileMonitorCommandScan(Envelope));
        }
    }
}