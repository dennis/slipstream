#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class FileMonitorFileRenamed : IEvent
    {
        public string EventType => "FileMonitorFileRenamed";
        public bool ExcludeFromTxrx => true;
        public string? FilePath { get; set; }
        public string? OldFilePath { get; set; }
    }
}
