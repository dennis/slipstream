using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingTime : IEvent
    {
        public string EventType => nameof(IRacingTime);

        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();

        public double SessionTime { get; set; }
        public double SessionTimeRemaining { get; set; }
    }
}