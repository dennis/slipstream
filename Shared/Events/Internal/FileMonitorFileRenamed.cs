#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class FileMonitorFileRenamed : IEvent
    {
        public string? FilePath { get; set; }
        public string? OldFilePath { get; set; }
    }
}
