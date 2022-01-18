using Slipstream.Components.IRacing.Models;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.IRacing.Trackers
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

        public void Handle(GameState.IState currentState, IRacingDataTrackerState state, IEventEnvelope envelope)
        {
            foreach (var car in currentState.Cars)
            {
                if (!state.CarsTracked.TryGetValue(car.CarIdx, out CarState? carState))
                {
                    carState = CarState.Build(car.CarIdx, currentState);
                    state.CarsTracked.Add(car.CarIdx, carState);
                }

                Events.IRacingCarInfo @event = GenerateIRacingCarInfoEvent(currentState, envelope, car);

                if (!carState.CarInfo.SameAs(@event))
                {
                    EventBus.PublishEvent(@event);

                    carState.CarInfo = @event;
                }
            }
        }

        private Events.IRacingCarInfo GenerateIRacingCarInfoEvent(GameState.IState currentState, IEventEnvelope envelope, GameState.Car car)
        {
            return EventFactory.CreateIRacingCarInfo(
                envelope: envelope,
                sessionTime: currentState.SessionTime,
                carIdx: car.CarIdx,
                carNumber: car.CarNumber,
                currentDriverUserID: car.UserId,
                currentDriverName: car.UserName,
                teamID: car.TeamId,
                teamName: car.TeamName,
                carName: car.CarName,
                carNameShort: car.CarNameShort,
                currentDriverIRating: car.IRating,
                currentDriverLicense: car.License,
                localUser: currentState.DriverCarIdx == car.CarIdx,
                spectator: car.IsSpectator
            );
        }

        public void Request(GameState.IState currentState, IRacingDataTrackerState state, IEventEnvelope envelope, IIRacingDataTracker.RequestType request)
        {
            if (request != IIRacingDataTracker.RequestType.CarInfo)
                return;

            foreach (var car in currentState.Cars)
            {
                if (!state.CarsTracked.TryGetValue(car.CarIdx, out _))
                {
                    CarState carState = CarState.Build(car.CarIdx, currentState);
                    state.CarsTracked.Add(car.CarIdx, carState);
                }

                Events.IRacingCarInfo @event = GenerateIRacingCarInfoEvent(currentState, envelope, car);
                EventBus.PublishEvent(@event);
            }
        }
    }
}