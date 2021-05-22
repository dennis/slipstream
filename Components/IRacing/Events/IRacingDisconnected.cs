using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingDisconnected : IEvent
    {
        public string EventType => "IRacingDisconnected";
        public ulong Uptime { get; set; }

        public override bool Equals(object obj)
        {
            return obj is IRacingDisconnected disconnected &&
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