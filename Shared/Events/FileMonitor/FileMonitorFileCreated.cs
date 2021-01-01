#nullable enable

namespace Slipstream.Shared.Events.FileMonitor
{
    public class FileMonitorFileCreated : IEvent
    {
        public string EventType => "FileMonitorFileCreated";
        public bool ExcludeFromTxrx => true;
        public string? FilePath { get; set; }
    }
}
