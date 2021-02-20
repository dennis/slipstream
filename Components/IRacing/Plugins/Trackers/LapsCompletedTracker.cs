using Slipstream.Components.IRacing.Plugins.Models;
using Slipstream.Shared;
using System.Diagnostics;

#nullable enable

namespace Slipstream.Components.IRacing.Plugins.Trackers
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
                    Debug.WriteLine($"{now} Added car: {car.CarIdx} at {car.Location}");

                    lapState = new LapState
                    {
                        Location = car.Location,
                        LapsCompleted = lapsCompleted,
                        TimingEnabled = false,
                    };

                    state.Laps.Add(car.CarIdx, lapState);
                }
                else
                {
                    if (car.Location == IIRacingEventFactory.CarLocation.NotInWorld)
                    {
                        lapState.ConsecutiveNotInWorld += 1;
                    }
                    else
                    {
                        lapState.ConsecutiveNotInWorld = 0;
                    }

                    if (lapState.Location == IIRacingEventFactory.CarLocation.InPitStall && car.Location == IIRacingEventFactory.CarLocation.AproachingPits)
                    {
                        Debug.WriteLine($"{now} Car-{car.CarIdx} exiting pits. enabled timing as of {now} lapState.LapsCompleted={lapState.LapsCompleted}, lapsCompleted={lapsCompleted}");
                        lapState.TimingEnabled = true;
                        lapState.CurrentLapStartTime = now;
                    }
                    else if (lapState.TimingEnabled && car.Location == IIRacingEventFactory.CarLocation.NotInWorld && lapState.ConsecutiveNotInWorld > 1)
                    {
                        Debug.WriteLine($"{now} Car-{car.CarIdx} Resetting - disable timings. Using 0 as CurrentLapStartTime");
                        lapState.TimingEnabled = false;
                    }
                    else if (lapsCompleted != lapState.LapsCompleted && lapState.LapsCompleted != -1)
                    {
                        // if previous lap was -1, then this is the initial lap and there was no previous lap, hence no timing is available

                        Debug.WriteLine($"{now} Car-{car.CarIdx} new lap lapsCompleted={lapsCompleted}, lapState.LapsCompleted={lapState.LapsCompleted}, lapState.TimingEnabled={lapState.TimingEnabled}");

                        lapState.OurLapTimeMeasurement = now - lapState.CurrentLapStartTime;
                        lapState.CurrentLapStartTime = now;
                        lapState.PendingLapTime = lapState.TimingEnabled && lapState.LapsCompleted != -1; ;
                        lapState.TimingEnabled = true;

                        if (localUser)
                        {
                            lapState.LastLapFuelDelta = currentState.FuelLevel - lapState.FuelLevelAtLapStart;
                        }
                        else
                        {
                            lapState.LastLapFuelDelta = null;
                        }

                        lapState.FuelLevelAtLapStart = currentState.FuelLevel;
                    }

                    if (lapState.TimingEnabled && lapState.PendingLapTime && (lapState.CurrentLapStartTime + 3) <= now)
                    {
                        // If possible we will use the timing provided by IRacing. These are updated after approx 2.5s
                        // after we cross s/f line. In case of incidents, no laptime will be provided, and we'll fall back
                        // to our own timing (which can be pretty inaccurate)

                        Debug.WriteLine($"{now} Car-{car.CarIdx}. Pending time. 3s delay localUser={localUser}, car.LastLapTime={car.LastLapTime}, lapState.OurLapTimeMeasurement={lapState.OurLapTimeMeasurement}");

                        var time = car.LastLapTime;

                        if (time == -1)
                        {
                            Debug.WriteLine($"{now} Car-{car.CarIdx}. Using own timing");
                            time = lapState.OurLapTimeMeasurement;
                        }

                        var @event = EventFactory.CreateIRacingCompletedLap(
                            sessionTime: now,
                            carIdx: car.CarIdx,
                            time: time,
                            lapsCompleted: lapsCompleted,
                            fuelDiff: lapState.LastLapFuelDelta,
                            localUser: localUser
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