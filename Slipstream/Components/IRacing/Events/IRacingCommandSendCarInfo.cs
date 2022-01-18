using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingCommandSendCarInfo : IEvent
    {
        public string EventType => nameof(IRacingCommandSendCarInfo);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
    }
}