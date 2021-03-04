#nullable enable

using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingPitstopReport : IEvent
    {
        public string EventType => "IRacingPitstopReport";
        public bool ExcludeFromTxrx => false;
        public ulong Uptime { get; set; }
        public double SessionTime { get; set; }
        public long CarIdx { get; set; }

        #region Temperature: Left Front

        public uint TempLFL { get; set; }
        public uint TempLFM { get; set; }
        public uint TempLFR { get; set; }

        #endregion Temperature: Left Front

        #region Temperature: Right Front

        public uint TempRFL { get; set; }
        public uint TempRFM { get; set; }
        public uint TempRFR { get; set; }

        #endregion Temperature: Right Front

        #region Temperature: Left Rear

        public uint TempLRL { get; set; }
        public uint TempLRM { get; set; }
        public uint TempLRR { get; set; }

        #endregion Temperature: Left Rear

        #region Temperature: Right Rear

        public uint TempRRL { get; set; }
        public uint TempRRM { get; set; }
        public uint TempRRR { get; set; }

        #endregion Temperature: Right Rear

        #region Wear: Left Front

        public uint WearLFL { get; set; }
        public uint WearLFM { get; set; }
        public uint WearLFR { get; set; }

        #endregion Wear: Left Front

        #region Wear: Right Front

        public uint WearRFL { get; set; }
        public uint WearRFM { get; set; }
        public uint WearRFR { get; set; }

        #endregion Wear: Right Front

        #region Wear: Left Rear

        public uint WearLRL { get; set; }
        public uint WearLRM { get; set; }
        public uint WearLRR { get; set; }

        #endregion Wear: Left Rear

        #region Wear: Right Rear

        public uint WearRRL { get; set; }
        public uint WearRRM { get; set; }
        public uint WearRRR { get; set; }

        #endregion Wear: Right Rear

        public long Laps { get; set; }
        public float FuelDelta { get; set; }
        public double Duration { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is IRacingPitstopReport report &&
                   EventType == report.EventType &&
                   ExcludeFromTxrx == report.ExcludeFromTxrx &&
                   SessionTime == report.SessionTime &&
                   CarIdx == report.CarIdx &&
                   TempLFL == report.TempLFL &&
                   TempLFM == report.TempLFM &&
                   TempLFR == report.TempLFR &&
                   TempRFL == report.TempRFL &&
                   TempRFM == report.TempRFM &&
                   TempRFR == report.TempRFR &&
                   TempLRL == report.TempLRL &&
                   TempLRM == report.TempLRM &&
                   TempLRR == report.TempLRR &&
                   TempRRL == report.TempRRL &&
                   TempRRM == report.TempRRM &&
                   TempRRR == report.TempRRR &&
                   WearLFL == report.WearLFL &&
                   WearLFM == report.WearLFM &&
                   WearLFR == report.WearLFR &&
                   WearRFL == report.WearRFL &&
                   WearRFM == report.WearRFM &&
                   WearRFR == report.WearRFR &&
                   WearLRL == report.WearLRL &&
                   WearLRM == report.WearLRM &&
                   WearLRR == report.WearLRR &&
                   WearRRL == report.WearRRL &&
                   WearRRM == report.WearRRM &&
                   WearRRR == report.WearRRR &&
                   Laps == report.Laps &&
                   FuelDelta == report.FuelDelta &&
                   Duration == report.Duration;
        }

        public override int GetHashCode()
        {
            int hashCode = 1299679726;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + SessionTime.GetHashCode();
            hashCode = hashCode * -1521134295 + CarIdx.GetHashCode();
            hashCode = hashCode * -1521134295 + TempLFL.GetHashCode();
            hashCode = hashCode * -1521134295 + TempLFM.GetHashCode();
            hashCode = hashCode * -1521134295 + TempLFR.GetHashCode();
            hashCode = hashCode * -1521134295 + TempRFL.GetHashCode();
            hashCode = hashCode * -1521134295 + TempRFM.GetHashCode();
            hashCode = hashCode * -1521134295 + TempRFR.GetHashCode();
            hashCode = hashCode * -1521134295 + TempLRL.GetHashCode();
            hashCode = hashCode * -1521134295 + TempLRM.GetHashCode();
            hashCode = hashCode * -1521134295 + TempLRR.GetHashCode();
            hashCode = hashCode * -1521134295 + TempRRL.GetHashCode();
            hashCode = hashCode * -1521134295 + TempRRM.GetHashCode();
            hashCode = hashCode * -1521134295 + TempRRR.GetHashCode();
            hashCode = hashCode * -1521134295 + WearLFL.GetHashCode();
            hashCode = hashCode * -1521134295 + WearLFM.GetHashCode();
            hashCode = hashCode * -1521134295 + WearLFR.GetHashCode();
            hashCode = hashCode * -1521134295 + WearRFL.GetHashCode();
            hashCode = hashCode * -1521134295 + WearRFM.GetHashCode();
            hashCode = hashCode * -1521134295 + WearRFR.GetHashCode();
            hashCode = hashCode * -1521134295 + WearLRL.GetHashCode();
            hashCode = hashCode * -1521134295 + WearLRM.GetHashCode();
            hashCode = hashCode * -1521134295 + WearLRR.GetHashCode();
            hashCode = hashCode * -1521134295 + WearRRL.GetHashCode();
            hashCode = hashCode * -1521134295 + WearRRM.GetHashCode();
            hashCode = hashCode * -1521134295 + WearRRR.GetHashCode();
            hashCode = hashCode * -1521134295 + Laps.GetHashCode();
            hashCode = hashCode * -1521134295 + FuelDelta.GetHashCode();
            hashCode = hashCode * -1521134295 + Duration.GetHashCode();
            return hashCode;
        }
    }
}