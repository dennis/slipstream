#nullable enable

namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingSessionState : IEvent
    {
        public string EventType => "IRacingSessionState";
        public bool ExcludeFromTxrx => false;
        public double SessionTime { get; set; }
        public string State { get; set; } = "";

        public bool DifferentTo(IRacingSessionState other)
        {
            return State.Equals(other.State);
        }
    }
}
