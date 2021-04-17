using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.Twitch.Events
{
    public class TwitchConnected : IEvent
    {
        public string EventType => "TwitchConnected";
        public ulong Uptime { get; set; }
        public string InstanceId { get; set; } = string.Empty;

        public override bool Equals(object obj)
        {
            return obj is TwitchConnected connected &&
                   InstanceId == connected.InstanceId &&
                   EventType == connected.EventType;
        }

        public override int GetHashCode()
        {
            int hashCode = -441302714;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(InstanceId);
            return hashCode;
        }
    }
}