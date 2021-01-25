#nullable enable

using System.Collections.Generic;

namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingCarPosition : IEvent
    {
        public string EventType => "IRacingCarPosition";
        public bool ExcludeFromTxrx => false;
        public ulong Uptime { get; set; }
        public double SessionTime { get; set; }
        public long CarIdx { get; set; }
        public bool LocalUser { get; set; }
        public int PositionInClass { get; set; }
        public int PositionInRace { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is IRacingCarPosition position &&
                   EventType == position.EventType &&
                   ExcludeFromTxrx == position.ExcludeFromTxrx &&
                   SessionTime == position.SessionTime &&
                   CarIdx == position.CarIdx &&
                   LocalUser == position.LocalUser &&
                   PositionInClass == position.PositionInClass &&
                   PositionInRace == position.PositionInRace;
        }

        public override int GetHashCode()
        {
            int hashCode = 1686385135;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + SessionTime.GetHashCode();
            hashCode = hashCode * -1521134295 + CarIdx.GetHashCode();
            hashCode = hashCode * -1521134295 + LocalUser.GetHashCode();
            hashCode = hashCode * -1521134295 + PositionInClass.GetHashCode();
            hashCode = hashCode * -1521134295 + PositionInRace.GetHashCode();
            return hashCode;
        }
    }
}