using System.Collections.Generic;

namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingCommandSendRaceFlags : IEvent
    {
        public string EventType => "IRacingCommandSendRaceFlags";
        public bool ExcludeFromTxrx => false;

        public override bool Equals(object obj)
        {
            return obj is IRacingCommandSendRaceFlags flags &&
                   EventType == flags.EventType &&
                   ExcludeFromTxrx == flags.ExcludeFromTxrx;
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
