using System.Collections.Generic;

namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingCommandSendCurrentSession : IEvent
    {
        public string EventType => "IRacingCommandSendCurrentSession";
        public bool ExcludeFromTxrx => false;
        public ulong Uptime { get; set; }

        public override bool Equals(object obj)
        {
            return obj is IRacingCommandSendCurrentSession session &&
                   EventType == session.EventType &&
                   ExcludeFromTxrx == session.ExcludeFromTxrx;
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
