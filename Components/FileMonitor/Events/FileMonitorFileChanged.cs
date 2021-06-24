#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.FileMonitor.Events
{
    public class FileMonitorFileChanged : IEvent
    {
        public string EventType => nameof(FileMonitorFileChanged);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public string FilePath { get; set; } = string.Empty;
    }
}