using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingCommandPitCleanWindshield : IEvent
    {
        public string EventType => nameof(IRacingCommandPitCleanWindshield);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
    }
}