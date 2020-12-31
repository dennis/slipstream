#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class FileMonitorScanCompleted : IEvent
    {
        public string EventType => "FileMonitorScanCompleted";
        public bool ExcludeFromTxrx => true;
    }
}
