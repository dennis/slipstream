using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingCompletedLap : IEvent
    {
        public string EventType => nameof(IRacingCompletedLap);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public double SessionTime { get; set; }
        public long CarIdx { get; set; }
        public double LapTime { get; set; }
        public bool EstimatedLapTime { get; set; }
        public int LapsCompleted { get; set; }
        public float? FuelLeft { get; set; }
        public float? FuelDelta { get; set; }
        public bool LocalUser { get; set; }
        public bool BestLap { get; set; }
    }
}