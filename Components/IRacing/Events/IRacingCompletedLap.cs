using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingCompletedLap : IEvent
    {
        public string EventType => "IRacingCompletedLap";
        public bool ExcludeFromTxrx => false;
        public ulong Uptime { get; set; }
        public double SessionTime { get; set; }
        public long CarIdx { get; set; }
        public double LapTime { get; set; }
        public bool EstimatedLapTime { get; set; }
        public int LapsCompleted { get; set; }
        public float? FuelDelta { get; set; }
        public bool LocalUser { get; set; }
        public bool BestLap { get; set; }

        public override bool Equals(object obj)
        {
            return obj is IRacingCompletedLap lap &&
                   EventType == lap.EventType &&
                   ExcludeFromTxrx == lap.ExcludeFromTxrx &&
                   SessionTime == lap.SessionTime &&
                   CarIdx == lap.CarIdx &&
                   LapTime == lap.LapTime &&
                   EstimatedLapTime == lap.EstimatedLapTime &&
                   LapsCompleted == lap.LapsCompleted &&
                   FuelDelta == lap.FuelDelta &&
                   LocalUser == lap.LocalUser &&
                   BestLap == lap.BestLap;
        }

        public override int GetHashCode()
        {
            int hashCode = 508366613;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + SessionTime.GetHashCode();
            hashCode = hashCode * -1521134295 + CarIdx.GetHashCode();
            hashCode = hashCode * -1521134295 + LapTime.GetHashCode();
            hashCode = hashCode * -1521134295 + EstimatedLapTime.GetHashCode();
            hashCode = hashCode * -1521134295 + LapsCompleted.GetHashCode();
            hashCode = hashCode * -1521134295 + FuelDelta.GetHashCode();
            hashCode = hashCode * -1521134295 + LocalUser.GetHashCode();
            hashCode = hashCode * -1521134295 + BestLap.GetHashCode();
            return hashCode;
        }
    }
}