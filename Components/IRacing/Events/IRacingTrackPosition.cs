#nullable enable

using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingTrackPosition : IEvent
    {
        public string EventType => "IRacingTrackPosition";
        public bool ExcludeFromTxrx => false;
        public ulong Uptime { get; set; }
        public double SessionTime { get; set; }
        public long CarIdx { get; set; }
        public bool LocalUser { get; set; }
        public int CurrentPositionInClass { get; set; }
        public int CurrentPositionInRace { get; set; }
        public int PreviousPositionInClass { get; set; }
        public int PreviousPositionInRace { get; set; }
        public int[] NewCarsAhead { get; set; } = new int[] { };
        public int[] NewCarsBehind { get; set; } = new int[] { };

        public override bool Equals(object? obj)
        {
            return obj is IRacingTrackPosition position &&
                   EventType == position.EventType &&
                   ExcludeFromTxrx == position.ExcludeFromTxrx &&
                   SessionTime == position.SessionTime &&
                   CarIdx == position.CarIdx &&
                   LocalUser == position.LocalUser &&
                   CurrentPositionInClass == position.CurrentPositionInClass &&
                   CurrentPositionInRace == position.CurrentPositionInRace &&
                   PreviousPositionInClass == position.PreviousPositionInClass &&
                   PreviousPositionInRace == position.PreviousPositionInRace &&
                   EqualityComparer<int[]>.Default.Equals(NewCarsAhead, position.NewCarsAhead) &&
                   EqualityComparer<int[]>.Default.Equals(NewCarsBehind, position.NewCarsBehind);
        }

        public override int GetHashCode()
        {
            int hashCode = -317246058;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + SessionTime.GetHashCode();
            hashCode = hashCode * -1521134295 + CarIdx.GetHashCode();
            hashCode = hashCode * -1521134295 + LocalUser.GetHashCode();
            hashCode = hashCode * -1521134295 + CurrentPositionInClass.GetHashCode();
            hashCode = hashCode * -1521134295 + CurrentPositionInRace.GetHashCode();
            hashCode = hashCode * -1521134295 + PreviousPositionInClass.GetHashCode();
            hashCode = hashCode * -1521134295 + PreviousPositionInRace.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<int[]>.Default.GetHashCode(NewCarsAhead);
            hashCode = hashCode * -1521134295 + EqualityComparer<int[]>.Default.GetHashCode(NewCarsBehind);
            return hashCode;
        }
    }
}