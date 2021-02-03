using iRacingSDK;
using Slipstream.Components.IRacing.Plugins.Models;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.IRacing.Plugins.Trackers
{
    internal class CarInfoTracker : IIRacingDataTracker
    {
        private readonly IIRacingEventFactory EventFactory;
        private readonly IEventBus EventBus;

        public CarInfoTracker(IEventBus eventBus, IIRacingEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void Handle(DataSample data, IRacingDataTrackerState state)
        {
            foreach (var driver in data.SessionData.DriverInfo.Drivers)
            {
                //if (driver.IsPaceCar) - this doesn't work when doing test sessions. Here we will be flagged as Pace car
                //    continue;

                if (!state.CarsTracked.TryGetValue(driver.CarIdx, out CarState carState))
                {
                    carState = CarState.Build(driver.CarIdx, data);
                    state.CarsTracked.Add(driver.CarIdx, carState);
                }

                // TODO: Also add: DriverIncidentCount, TeamIncidentCount
                var @event = EventFactory.CreateIRacingCarInfo(
                    sessionTime: data.Telemetry.SessionTime,
                    carIdx: driver.CarIdx,
                    carNumber: driver.CarNumber,
                    currentDriverUserID: driver.UserID,
                    currentDriverName: driver.UserName,
                    teamID: driver.TeamID,
                    teamName: driver.TeamName,
                    carName: driver.CarScreenName,
                    carNameShort: driver.CarScreenNameShort,
                    currentDriverIRating: driver.IRating,
                    currentDriverLicense: driver.LicString,
                    localUser: data.SessionData.DriverInfo.DriverCarIdx == driver.CarIdx,
                    spectator: driver.IsSpectator != 0
                );

                if (!carState.CarInfo.SameAs(@event) || state.SendCarInfo)
                {
                    EventBus.PublishEvent(@event);

                    carState.CarInfo = @event;
                }
            }

            state.SendCarInfo = false;
        }
    }
}