using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingConnected : IEvent
    {
        public string EventType => nameof(IRacingConnected);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
    }
}