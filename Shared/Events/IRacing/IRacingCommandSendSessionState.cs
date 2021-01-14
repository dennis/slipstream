using System.Collections.Generic;

namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingCommandSendSessionState : IEvent
    {
        public string EventType => "IRacingCommandSendSessionState";
        public bool ExcludeFromTxrx => false;
        public ulong Uptime { get; set; }

        public override bool Equals(object obj)
        {
            return obj is IRacingCommandSendSessionState state &&
                   EventType == state.EventType &&
                   ExcludeFromTxrx == state.ExcludeFromTxrx;
        }

        public override int GetHashCode()
        {
            int hashCode = -441302714;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            return hashCode;
        }
    }
}
