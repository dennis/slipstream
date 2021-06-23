using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingCommandPitClearAll : IEvent
    {
        public string EventType => nameof(IRacingCommandPitClearAll);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
    }
}