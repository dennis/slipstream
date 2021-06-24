using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingCommandSendTrackInfo : IEvent
    {
        public string EventType => nameof(IRacingCommandSendTrackInfo);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
    }
}