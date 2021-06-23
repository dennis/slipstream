#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.FileMonitor.Events
{
    public class FileMonitorCommandScan : IEvent
    {
        public string EventType => nameof(FileMonitorCommandScan);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
    }
}