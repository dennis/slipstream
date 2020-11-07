#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class FileMonitorFileChanged : IEvent
    {
        public string EventType => "FileMonitorFileChanged";
        public string? FilePath { get; set; }
    }
}
