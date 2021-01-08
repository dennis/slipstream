using System.Collections.Generic;

namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingCommandSendWeatherInfo : IEvent
    {
        public string EventType => "IRacingCommandSendWeatherInfo";
        public bool ExcludeFromTxrx => false;

        public override bool Equals(object obj)
        {
            return obj is IRacingCommandSendWeatherInfo info &&
                   EventType == info.EventType &&
                   ExcludeFromTxrx == info.ExcludeFromTxrx;
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
