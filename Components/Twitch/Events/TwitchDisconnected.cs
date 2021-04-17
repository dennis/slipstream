using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.Twitch.Events
{
    public class TwitchDisconnected : IEvent
    {
        public string EventType => "TwitchDisconnected";
        public ulong Uptime { get; set; }

        public override bool Equals(object obj)
        {
            return obj is TwitchDisconnected disconnected &&
                   EventType == disconnected.EventType;
        }

        public override int GetHashCode()
        {
            int hashCode = -441302714;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            return hashCode;
        }
    }
}