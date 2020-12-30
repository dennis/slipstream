#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class FileMonitorFileDeleted : IEvent
    {
        public string EventType => "FileMonitorFileDeleted";
        public bool ExcludeFromTxrx => true;
        public string? FilePath { get; set; }
    }
}
