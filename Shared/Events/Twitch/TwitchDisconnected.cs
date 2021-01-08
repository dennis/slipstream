using System.Collections.Generic;

namespace Slipstream.Shared.Events.Twitch
{
    public class TwitchDisconnected : IEvent
    {
        public string EventType => "TwitchDisconnected";
        public bool ExcludeFromTxrx => false;

        public override bool Equals(object obj)
        {
            return obj is TwitchDisconnected disconnected &&
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
