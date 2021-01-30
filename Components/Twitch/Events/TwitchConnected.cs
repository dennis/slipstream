using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.Twitch.Events
{
    public class TwitchConnected : IEvent
    {
        public string EventType => "TwitchConnected";
        public bool ExcludeFromTxrx => false;
        public ulong Uptime { get; set; }

        public override bool Equals(object obj)
        {
            return obj is TwitchConnected connected &&
                   EventType == connected.EventType &&
                   ExcludeFromTxrx == connected.ExcludeFromTxrx;
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