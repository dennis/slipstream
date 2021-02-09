using Slipstream.Components.IRacing.Events;

namespace Slipstream.Components.IRacing.Plugins.Models
{
    internal class CarState
    {
        public bool LastOnPitRoad { get; set; }
        public int LastLap { get; set; }
        public double LastLapTime { get; set; }
        public float FuelLevelLastLap { get; set; }
        public IRacingCarInfo CarInfo { get; set; } = new IRacingCarInfo();
        public int ObservedCrossFinishingLine { get; set; }
        public double? PitEnteredAt { get; set; }
        public int StintStartLap { get; set; }
        public float StintFuelLevel { get; set; }
        public double StintStartTime { get; set; }

        public static CarState Build(long idx, GameState.IState gameState)
        {
            return new CarState
            {
                StintStartLap = gameState.Cars[idx].LapsCompleted,
                StintFuelLevel = gameState.FuelLevel, // FIXME: This shouldn't be set, unless it's our car
                StintStartTime = gameState.SessionTime
            };
        }

        public void ClearState() // This is invoked at start of session, so we set ObservedCrossFinishingLine higher than initially
        {
            LastOnPitRoad = false;
            LastLap = -1;
            FuelLevelLastLap = -1;
            ObservedCrossFinishingLine = 2; // as we are starting on lap 0, we need a full lap before reporting
            PitEnteredAt = null;
            StintFuelLevel = -1;
            StintStartTime = 0;
        }
    }
}