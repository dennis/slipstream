using System.Collections.Generic;

namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingDisconnected : IEvent
    {
        public string EventType => "IRacingDisconnected";
        public bool ExcludeFromTxrx => false;

        public override bool Equals(object obj)
        {
            return obj is IRacingDisconnected disconnected &&
                   EventType == disconnected.EventType &&
                   ExcludeFromTxrx == disconnected.ExcludeFromTxrx;
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
