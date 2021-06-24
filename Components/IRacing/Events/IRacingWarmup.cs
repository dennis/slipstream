using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingWarmup : IIRacingSessionState
    {
        public string EventType => nameof(IRacingWarmup);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public double SessionTime { get; set; }
        public bool LapsLimited { get; set; }
        public bool TimeLimited { get; set; }
        public double TotalSessionTime { get; set; }
        public int TotalSessionLaps { get; set; }
        public string State { get; set; } = string.Empty; // Checkered, CoolDown, GetInChar, ParadeLaps, Racing, Warmup
        public string Category { get; set; } = string.Empty; // Road, Oval, DirtRoad, DirtOval
    }
}