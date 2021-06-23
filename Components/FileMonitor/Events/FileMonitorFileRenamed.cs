#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.FileMonitor.Events
{
    public class FileMonitorFileRenamed : IEvent
    {
        public string EventType => nameof(FileMonitorFileRenamed);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public string FilePath { get; set; } = string.Empty;
        public string OldFilePath { get; set; } = string.Empty;
    }
}