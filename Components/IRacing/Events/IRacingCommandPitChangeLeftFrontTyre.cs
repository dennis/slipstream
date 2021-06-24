using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingCommandPitChangeLeftFrontTyre : IEvent
    {
        public string EventType => nameof(IRacingCommandPitChangeLeftFrontTyre);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public int Kpa { get; set; }
    }
}