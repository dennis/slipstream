using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingCommandSendSessionState : IEvent
    {
        public string EventType => "IRacingCommandSendSessionState";
        public ulong Uptime { get; set; }

        public override bool Equals(object obj)
        {
            return obj is IRacingCommandSendSessionState state &&
                   EventType == state.EventType;
        }

        public override int GetHashCode()
        {
            int hashCode = -441302714;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            return hashCode;
        }
    }
}