using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingDisconnected : IEvent
    {
        public string EventType => nameof(IRacingDisconnected);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
    }
}