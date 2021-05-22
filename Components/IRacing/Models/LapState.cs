namespace Slipstream.Components.IRacing.Models
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
        public int LastSessionNum { get; set; }

        public void Clear()
        {
            Location = IIRacingEventFactory.CarLocation.NotInWorld;
            TimingEnabled = false;
            CurrentLapStartTime = 0;
            LapsCompleted = -1;
            OurLapTimeMeasurement = 0;
            PendingLapTime = false;
            FuelLevelAtLapStart = 0;
            LastLapFuelDelta = 0;
            ConsecutiveNotInWorld = 0;
            LastSessionNum = -1;
        }
    }
}