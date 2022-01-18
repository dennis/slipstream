#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.FileMonitor.Events
{
    public class FileMonitorScanCompleted : IEvent
    {
        public string EventType => nameof(FileMonitorScanCompleted);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
    }
}