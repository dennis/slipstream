using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingCommandPitRequestFastRepair : IEvent
    {
        public string EventType => nameof(IRacingCommandPitRequestFastRepair);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
    }
}