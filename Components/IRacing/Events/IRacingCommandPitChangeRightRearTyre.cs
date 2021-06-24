using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingCommandPitChangeRightRearTyre : IEvent
    {
        public string EventType => nameof(IRacingCommandPitChangeRightRearTyre);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public int Kpa { get; set; }
    }
}