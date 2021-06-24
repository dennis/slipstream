using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingCommandSendSessionState : IEvent
    {
        public string EventType => nameof(IRacingCommandSendSessionState);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
    }
}