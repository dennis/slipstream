#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingDriverIncident : IEvent
    {
        public string EventType => nameof(IRacingDriverIncident);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public int DriverIncidentCount { get; set; }
        public int DriverIncidentDelta { get; set; }
        public int TeamIncidentCount { get; set; }
        public int TeamIncidentDelta { get; set; }
        public int MyIncidentCount { get; set; }
        public int MyIncidentDelta { get; set; }
    }
}