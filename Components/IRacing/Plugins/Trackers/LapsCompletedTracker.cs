using iRacingSDK;
using Slipstream.Components.IRacing.Plugins.Models;
using Slipstream.Shared;
using System.Linq;

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

        public void Handle(DataSample data, IRacingDataTrackerState state)
        {
            // We cant use data.Telemetry.Cars[].LastTime, as it wont contain data if
            // the driver had an incident. So we calculate it ourself which will be slightly
            // off compared to IRacings own timing.
            // If you plan to use LastTime, please remember to wait approx 2.5 before reading
            // LastTime, as the value is updated with about 2.5s delay
            var now = data.Telemetry.SessionTime;

            for (int i = 0; i < data.Telemetry.Cars.Count(); i++)
            {
                if (!state.CarsTracked.TryGetValue(i, out CarState carState))
                {
                    carState = CarState.Build(i, data);
                    state.CarsTracked.Add(i, carState);
                }

                var lapsCompleted = data.Telemetry.CarIdxLapCompleted[i];

                if (lapsCompleted != -1 && carState.LastLap != lapsCompleted)
                {
                    carState.ObservedCrossFinishingLine++;
                    if (lapsCompleted == 0) // This is initial lap, so we can use times from next lap
                        carState.ObservedCrossFinishingLine++;

                    bool localUser = i == data.SessionData.DriverInfo.DriverCarIdx;

                    // 1st time is when leaving the pits / or whatever lap it is on when we join
                    // 2nd time is start of first real lap (that we see in full)
                    // 3rd+ is lap times (we can begin timing laps)
                    // if "we" are doing laps, then we know we joined with the car,
                    // so we can track laps from the 1st one.
                    // for everybody else, we need to see them crossing the line 3 or more times as describe above
                    if (carState.ObservedCrossFinishingLine >= 3 || (localUser && lapsCompleted > 0))
                    {
                        var lapTime = now - carState.LastLapTime;

                        var fuelLeft = data.Telemetry.FuelLevel;

                        float? usedFuel = fuelLeft - carState.FuelLevelLastLap;
                        carState.FuelLevelLastLap = fuelLeft;

                        var @event = EventFactory.CreateIRacingCarCompletedLap(
                            sessionTime: now,
                            carIdx: i,
                            time: lapTime,
                            lapsCompleted: lapsCompleted,
                            fuelDiff: localUser ? usedFuel : null,
                            localUser: localUser);

                        EventBus.PublishEvent(@event);
                    }
                    carState.LastLapTime = now;
                    carState.LastLap = lapsCompleted;
                }
            }
        }
    }
}