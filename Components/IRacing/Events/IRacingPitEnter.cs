using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingPitEnter : IEvent
    {
        public string EventType => nameof(IRacingPitEnter);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public double SessionTime { get; set; }
        public long CarIdx { get; set; }
        public bool LocalUser { get; set; }
    }
}