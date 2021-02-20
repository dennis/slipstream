namespace Slipstream.Components.IRacing.Plugins.Models
{
    internal class LapState
    {
        public IIRacingEventFactory.CarLocation Location { get; set; }
        public bool TimingEnabled { get; set; }
        public double CurrentLapStartTime { get; set; }
        public int LapsCompleted { get; set; }
        public double OurLapTimeMeasurement { get; set; }
        public bool PendingLapTime { get; set; }
        public float FuelLevelAtLapStart { get; set; }
        public float? LastLapFuelDelta { get; set; }
        public int ConsecutiveNotInWorld { get; set; }
    }
}