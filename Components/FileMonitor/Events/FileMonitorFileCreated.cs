#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.FileMonitor.Events
{
    public class FileMonitorFileCreated : IEvent
    {
        public string EventType => nameof(FileMonitorFileCreated);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public string FilePath { get; set; } = string.Empty;
    }
}