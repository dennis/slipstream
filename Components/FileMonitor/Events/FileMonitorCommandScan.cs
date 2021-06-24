#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.FileMonitor.Events
{
    public class FileMonitorCommandScan : IEvent
    {
        public string EventType => nameof(FileMonitorCommandScan);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
    }
}