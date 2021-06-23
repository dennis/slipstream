#nullable enable

using Slipstream.Components.IRacing.GameState;
using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingRaw : IEvent
    {
        public string EventType => nameof(IRacingRaw);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public IState CurrentState { get; set; } = new State();
    }
}