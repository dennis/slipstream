#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class FileMonitorFileCreated : IEvent
    {
        public string? FilePath { get; set; }
    }
}
