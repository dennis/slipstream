using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingPitEnter : IEvent
    {
        public string EventType => "IRacingPitEnter";
        public ulong Uptime { get; set; }
        public double SessionTime { get; set; }
        public long CarIdx { get; set; }
        public bool LocalUser { get; set; }

        public override bool Equals(object obj)
        {
            return obj is IRacingPitEnter enter &&
                   EventType == enter.EventType &&
                   SessionTime == enter.SessionTime &&
                   CarIdx == enter.CarIdx &&
                   LocalUser == enter.LocalUser;
        }

        public override int GetHashCode()
        {
            int hashCode = 2084919435;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + SessionTime.GetHashCode();
            hashCode = hashCode * -1521134295 + CarIdx.GetHashCode();
            hashCode = hashCode * -1521134295 + LocalUser.GetHashCode();
            return hashCode;
        }
    }
}