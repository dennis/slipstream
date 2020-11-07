#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class FileMonitorFileDeleted : IEvent
    {
        public string EventType => "FileMonitorFileDeleted";
        public string? FilePath { get; set; }
    }
}
