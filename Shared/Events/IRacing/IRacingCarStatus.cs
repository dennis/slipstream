#nullable enable

namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingCarStatus : IEvent
    {
        public string EventType => "IRacingCarStatus";
        public double SessionTime { get; set; }
        public long CarIdx { get; set; }

        #region Temperature: Left Front
        public float TempLFL { get; set; }
        public float TempLFM { get; set; }
        public float TempLFR { get; set; }
        #endregion

        #region Temperature: Right Front
        public float TempRFL { get; set; }
        public float TempRFM { get; set; }
        public float TempRFR { get; set; }
        #endregion

        #region Temperature: Left Rear
        public float TempLRL { get; set; }
        public float TempLRM { get; set; }
        public float TempLRR { get; set; }
        #endregion

        #region Temperature: Right Rear
        public float TempRRL { get; set; }
        public float TempRRM { get; set; }
        public float TempRRR { get; set; }
        #endregion

        #region Wear: Left Front
        public float WearLFL { get; set; }
        public float WearLFM { get; set; }
        public float WearLFR { get; set; }
        #endregion

        #region Wear: Right Front
        public float WearRFL { get; set; }
        public float WearRFM { get; set; }
        public float WearRFR { get; set; }
        #endregion

        #region Wear: Left Rear
        public float WearLRL { get; set; }
        public float WearLRM { get; set; }
        public float WearLRR { get; set; }
        #endregion

        #region Wear: Right Rear
        public float WearRRL { get; set; }
        public float WearRRM { get; set; }
        public float WearRRR { get; set; }
        #endregion
    }
}