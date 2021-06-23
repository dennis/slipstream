using Slipstream.Components.IRacing.Models;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.IRacing.Trackers
{
    internal class CarPositionTracker : IIRacingDataTracker
    {
        private readonly IIRacingEventFactory EventFactory;
        private readonly IEventBus EventBus;

        public CarPositionTracker(IEventBus eventBus, IIRacingEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void Handle(GameState.IState currentState, IRacingDataTrackerState state, IEventEnvelope envelope)
        {
            foreach (var car in currentState.Cars)
            {
                int positionInRace = car.ClassPosition;
                int positionInClass = car.Position;

                if (positionInRace > 0 && positionInClass > 0)
                {
                    if (positionInRace != state.LastPositionInRace[car.CarIdx] || positionInClass != state.LastPositionInClass[car.CarIdx])
                    {
                        var localUser = currentState.DriverCarIdx == car.CarIdx;

                        var @event = EventFactory.CreateIRacingCarPosition(
                            envelope: envelope,
                            sessionTime: currentState.SessionTime,
                            carIdx: car.CarIdx,
                            localUser: localUser,
                            positionInClass: positionInClass,
                            positionInRace: positionInRace);

                        EventBus.PublishEvent(@event);
                    }
                }

                state.LastPositionInClass[car.CarIdx] = positionInClass;
                state.LastPositionInRace[car.CarIdx] = positionInRace;
            }
        }
    }
}