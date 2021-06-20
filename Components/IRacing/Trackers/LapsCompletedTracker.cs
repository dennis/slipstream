using Slipstream.Components.IRacing.Models;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.IRacing.Trackers
{
    internal class LapsCompletedTracker : IIRacingDataTracker
    {
        private readonly IIRacingEventFactory EventFactory;
        private readonly IEventBus EventBus;

        public LapsCompletedTracker(IEventBus eventBus, IIRacingEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void Handle(GameState.IState currentState, IRacingDataTrackerState state)
        {
            var now = currentState.SessionTime;

            foreach (var car in currentState.Cars)
            {
                var lapsCompleted = car.LapsCompleted;
                var localUser = car.CarIdx == currentState.DriverCarIdx;

                if (!state.Laps.TryGetValue(car.CarIdx, out LapState lapState))
                {
                    lapState = new LapState
                    {
                        Location = car.Location,
                        LapsCompleted = lapsCompleted,
                        TimingEnabled = false,
                        LastSessionNum = currentState.SessionNum,
                    };

                    state.Laps.Add(car.CarIdx, lapState);
                }
                else
                {
                    if (lapState.LastSessionNum != currentState.SessionNum)
                    {
                        lapState.Clear();
                        lapState.LastSessionNum = currentState.SessionNum;
                    }

                    if (car.Location == IIRacingEventFactory.CarLocation.NotInWorld)
                    {
                        lapState.ConsecutiveNotInWorld++;
                    }
                    else
                    {
                        lapState.ConsecutiveNotInWorld = 0;
                    }

                    if (lapState.Location == IIRacingEventFactory.CarLocation.InPitStall && car.Location == IIRacingEventFactory.CarLocation.AproachingPits)
                    {
                        // Exiting pits
                        lapState.TimingEnabled = true;
                        lapState.CurrentLapStartTime = now;
                    }
                    else if (lapState.TimingEnabled && car.Location == IIRacingEventFactory.CarLocation.NotInWorld && lapState.ConsecutiveNotInWorld > 1)
                    {
                        // Car resetting
                        lapState.TimingEnabled = false;
                    }
                    else if (lapsCompleted != lapState.LapsCompleted && lapState.LapsCompleted != -1)
                    {
                        // New lap: if previous lap was -1, then this is the initial lap and there
                        // was no previous lap, hence no timing is available

                        lapState.OurLapTimeMeasurement = now - lapState.CurrentLapStartTime;
                        lapState.CurrentLapStartTime = now;
                        lapState.PendingLapTime = lapState.TimingEnabled && lapState.LapsCompleted != -1; ;
                        lapState.TimingEnabled = true;

                        if (localUser)
                        {
                            lapState.FuelLeft = currentState.FuelLevel;
                            lapState.LastLapFuelDelta = currentState.FuelLevel - lapState.FuelLevelAtLapStart;
                        }
                        else
                        {
                            lapState.FuelLeft = null;
                            lapState.LastLapFuelDelta = null;
                        }

                        lapState.FuelLevelAtLapStart = currentState.FuelLevel;
                    }

                    if (lapState.TimingEnabled && lapState.PendingLapTime && (lapState.CurrentLapStartTime + 3) <= now)
                    {
                        // If possible we will use the timing provided by IRacing. These are updated after approx 2.5s
                        // after we cross s/f line. In case of incidents, no laptime will be provided, and we'll fall back
                        // to our own timing (which can be pretty inaccurate)

                        var lapTime = car.LastLapTime;
                        var estimatedLapTime = false;

                        if (lapTime == -1)
                        {
                            lapTime = lapState.OurLapTimeMeasurement;
                            estimatedLapTime = true;
                        }

                        var @event = EventFactory.CreateIRacingCompletedLap(
                            sessionTime: now,
                            carIdx: car.CarIdx,
                            lapTime: lapTime,
                            estimatedLapTime: estimatedLapTime,
                            lapsCompleted: lapsCompleted,
                            fuelLeft: lapState.FuelLeft,
                            fuelDelta: lapState.LastLapFuelDelta,
                            localUser: localUser,
                            bestLap: car.BestLapNum == lapsCompleted
                        );

                        EventBus.PublishEvent(@event);

                        lapState.PendingLapTime = false;
                    }

                    lapState.LapsCompleted = lapsCompleted;
                    lapState.Location = car.Location;
                }
            }
        }
    }
}