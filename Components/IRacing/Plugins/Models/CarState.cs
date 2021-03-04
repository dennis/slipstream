using Slipstream.Components.IRacing.Events;

namespace Slipstream.Components.IRacing.Plugins.Models
{
    internal class CarState
    {
        public bool LapStartNotSeen { get; set; }
        public bool LastOnPitRoad { get; set; }
        public int LastLap { get; set; }
        public double LastLapStartedAt { get; set; }
        public float? FuelLevelAtLapStart { get; set; }
        public IRacingCarInfo CarInfo { get; set; } = new IRacingCarInfo();
        public int ObservedCrossFinishingLine { get; set; }
        public double? PitEnteredAt { get; set; }
        public int StintStartLap { get; set; }
        public float StintFuelLevel { get; set; }
        public double StintStartTime { get; set; }
        public double OurLaptTimeMeasurement { get; set; }
        public float? LastLapFuelDelta { get; internal set; }
        public bool PendingLapTime { get; internal set; }

        public static CarState Build(long idx, GameState.IState gameState)
        {
            var onTrack = false;
            int lapsCompleted = -1;

            if (gameState.Cars.Length > idx)
            {
                onTrack =
                    gameState.Cars[idx].Location == IIRacingEventFactory.CarLocation.AproachingPits ||
                    gameState.Cars[idx].Location == IIRacingEventFactory.CarLocation.OffTrack ||
                    gameState.Cars[idx].Location == IIRacingEventFactory.CarLocation.OnTrack;

                lapsCompleted = gameState.Cars[idx].LapsCompleted;
            }

            return new CarState
            {
                StintStartLap = lapsCompleted,
                StintFuelLevel = gameState.FuelLevel, // FIXME: This shouldn't be set, unless it's our car
                StintStartTime = gameState.SessionTime,
                LapStartNotSeen = gameState.DriverCarIdx != idx && onTrack,
            };
        }

        public void ClearState() // This is invoked at start of session, so we set ObservedCrossFinishingLine higher than initially
        {
            LastOnPitRoad = false;
            LastLap = -1;
            FuelLevelAtLapStart = -1;
            LapStartNotSeen = false;
            PitEnteredAt = null;
            StintFuelLevel = -1;
            StintStartTime = 0;
        }
    }
}