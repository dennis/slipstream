using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingCommandPitAddFuel : IEvent
    {
        public string EventType => nameof(IRacingCommandPitAddFuel);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public int AddLiters { get; set; }
    }
}