using Slipstream.Shared;

namespace Slipstream.Components.AppilcationUpdate.Events
{
    public class ApplicationUpdateCommandCheckLatestVersion : IEvent
    {
        public string EventType => nameof(ApplicationUpdateCommandCheckLatestVersion);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
    }
}