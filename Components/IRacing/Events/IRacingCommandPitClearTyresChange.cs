using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingCommandPitClearTyresChange : IEvent
    {
        public string EventType => nameof(IRacingCommandPitClearTyresChange);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
    }
}