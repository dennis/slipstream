#nullable enable

namespace Slipstream.Shared.Events.FileMonitor
{
    public class FileMonitorScanCompleted : IEvent
    {
        public string EventType => "FileMonitorScanCompleted";
        public bool ExcludeFromTxrx => true;
    }
}
