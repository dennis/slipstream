using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingCommandSendRaceFlags : IEvent
    {
        public string EventType => nameof(IRacingCommandSendRaceFlags);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
    }
}