#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.FileMonitor.Events
{
    public class FileMonitorFileDeleted : IEvent
    {
        public string EventType => nameof(FileMonitorFileDeleted);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public string FilePath { get; set; } = string.Empty;
    }
}