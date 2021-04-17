#nullable enable

using Slipstream.Components.IRacing.GameState;
using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingRaw : IEvent
    {
        public string EventType => "IRacingRaw";
        public bool ExcludeFromTxrx => false;
        public ulong Uptime { get; set; }
        public IState CurrentState { get; set; } = new State();
    }
}