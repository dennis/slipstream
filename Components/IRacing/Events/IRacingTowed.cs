using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingTowed : IEvent
    {
        public string EventType => nameof(IRacingTowed);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public double SessionTime { get; set; }
        public double RemainingTowTime { get; set; }
    }
}