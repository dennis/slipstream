#nullable enable

using System.Collections.Generic;

namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingSessionState : IEvent
    {
        public string EventType => "IRacingSessionState";
        public bool ExcludeFromTxrx => false;
        public ulong Uptime { get; set; }
        public double SessionTime { get; set; }
        public string State { get; set; } = "";

        public bool DifferentTo(IRacingSessionState other)
        {
            return State.Equals(other.State);
        }

        public override bool Equals(object? obj)
        {
            return obj is IRacingSessionState state &&
                   EventType == state.EventType &&
                   ExcludeFromTxrx == state.ExcludeFromTxrx &&
                   SessionTime == state.SessionTime &&
                   State == state.State;
        }

        public override int GetHashCode()
        {
            int hashCode = -1594495278;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + SessionTime.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(State);
            return hashCode;
        }
    }
}
