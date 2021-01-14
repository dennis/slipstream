#nullable enable

using System.Collections.Generic;

namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingPitExit : IEvent
    {
        public string EventType => "IRacingPitExit";
        public bool ExcludeFromTxrx => false;
        public ulong Uptime { get; set; }
        public double SessionTime { get; set; }
        public long CarIdx { get; set; }
        public bool LocalUser { get; set; }
        public double? Duration { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is IRacingPitExit exit &&
                   EventType == exit.EventType &&
                   ExcludeFromTxrx == exit.ExcludeFromTxrx &&
                   SessionTime == exit.SessionTime &&
                   CarIdx == exit.CarIdx &&
                   LocalUser == exit.LocalUser &&
                   Duration == exit.Duration;
        }

        public override int GetHashCode()
        {
            int hashCode = -1083203984;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + SessionTime.GetHashCode();
            hashCode = hashCode * -1521134295 + CarIdx.GetHashCode();
            hashCode = hashCode * -1521134295 + LocalUser.GetHashCode();
            hashCode = hashCode * -1521134295 + Duration.GetHashCode();
            return hashCode;
        }
    }
}
