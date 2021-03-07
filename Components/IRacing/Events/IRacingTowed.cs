using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingTowed : IEvent
    {
        public string EventType => "IRacingTowed";
        public bool ExcludeFromTxrx => false;
        public ulong Uptime { get; set; }
        public double SessionTime { get; set; }
        public double RemainingTowTime { get; set; }

        public override bool Equals(object obj)
        {
            return obj is IRacingTowed other &&
                   EventType == other.EventType &&
                   ExcludeFromTxrx == other.ExcludeFromTxrx &&
                   SessionTime == other.SessionTime &&
                   RemainingTowTime == other.RemainingTowTime;
        }

        public override int GetHashCode()
        {
            int hashCode = 2084919435;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + SessionTime.GetHashCode();
            hashCode = hashCode * -1521134295 + RemainingTowTime.GetHashCode();
            return hashCode;
        }
    }
}