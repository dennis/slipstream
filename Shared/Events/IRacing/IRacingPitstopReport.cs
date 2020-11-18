#nullable enable

namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingPitstopReport : IEvent
    {
        public string EventType => "IRacingPitstopReport";
        public double SessionTime { get; set; }
        public long CarIdx { get; set; }

        #region Temperature: Left Front
        public uint TempLFL { get; set; }
        public uint TempLFM { get; set; }
        public uint TempLFR { get; set; }
        #endregion

        #region Temperature: Right Front
        public uint TempRFL { get; set; }
        public uint TempRFM { get; set; }
        public uint TempRFR { get; set; }
        #endregion

        #region Temperature: Left Rear
        public uint TempLRL { get; set; }
        public uint TempLRM { get; set; }
        public uint TempLRR { get; set; }
        #endregion

        #region Temperature: Right Rear
        public uint TempRRL { get; set; }
        public uint TempRRM { get; set; }
        public uint TempRRR { get; set; }
        #endregion

        #region Wear: Left Front
        public uint WearLFL { get; set; }
        public uint WearLFM { get; set; }
        public uint WearLFR { get; set; }
        #endregion

        #region Wear: Right Front
        public uint WearRFL { get; set; }
        public uint WearRFM { get; set; }
        public uint WearRFR { get; set; }
        #endregion

        #region Wear: Left Rear
        public uint WearLRL { get; set; }
        public uint WearLRM { get; set; }
        public uint WearLRR { get; set; }
        #endregion

        #region Wear: Right Rear
        public uint WearRRL { get; set; }
        public uint WearRRM { get; set; }
        public uint WearRRR { get; set; }
        #endregion

        public long Laps { get; set; }
        public float FuelDiff { get; set; }
        public double Duration { get; set; }
    }
}