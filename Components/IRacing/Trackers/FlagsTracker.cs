using Slipstream.Components.IRacing.Events;
using Slipstream.Components.IRacing.GameState;
using Slipstream.Components.IRacing.Models;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.IRacing.Trackers
{
    internal class FlagsTracker : IIRacingDataTracker
    {
        private readonly IIRacingEventFactory EventFactory;
        private readonly IEventBus EventBus;

        public FlagsTracker(IEventBus eventBus, IIRacingEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void Handle(GameState.IState currentState, IRacingDataTrackerState state, IEventEnvelope envelope)
        {
            IRacingRaceFlags @event = GenerateEvent(currentState, envelope);

            if (state.LastRaceFlags?.DifferentTo(@event) != true)
            {
                if (@event.Green)
                {
                    // We can trust timings
                    foreach (var info in state.CarsTracked)
                    {
                        // Just start
                        info.Value.LapStartNotSeen = false;
                    }
                }

                EventBus.PublishEvent(@event);
                state.LastRaceFlags = @event;
            }
        }

        private IRacingRaceFlags GenerateEvent(IState currentState, IEventEnvelope envelope)
        {
            var sessionFlags = currentState.SessionFlags;

            return EventFactory.CreateIRacingRaceFlags
            (
                envelope: envelope,
                sessionTime: currentState.SessionTime,
                black: sessionFlags.HasFlag(SessionFlags.Black),
                blue: sessionFlags.HasFlag(SessionFlags.Blue),
                caution: sessionFlags.HasFlag(SessionFlags.Caution),
                cautionWaving: sessionFlags.HasFlag(SessionFlags.CautionWaving),
                checkered: sessionFlags.HasFlag(SessionFlags.Checkered),
                crossed: sessionFlags.HasFlag(SessionFlags.Crossed),
                debris: sessionFlags.HasFlag(SessionFlags.Debris),
                disqualify: sessionFlags.HasFlag(SessionFlags.Disqualify),
                fiveToGo: sessionFlags.HasFlag(SessionFlags.FiveToGo),
                furled: sessionFlags.HasFlag(SessionFlags.Furled),
                green: sessionFlags.HasFlag(SessionFlags.Green),
                greenHeld: sessionFlags.HasFlag(SessionFlags.GreenHeld),
                oneLapToGreen: sessionFlags.HasFlag(SessionFlags.OneLapToGreen),
                randomWaving: sessionFlags.HasFlag(SessionFlags.RandomWaving),
                red: sessionFlags.HasFlag(SessionFlags.Red),
                repair: sessionFlags.HasFlag(SessionFlags.Repair),
                servicible: sessionFlags.HasFlag(SessionFlags.Servicible),
                startGo: sessionFlags.HasFlag(SessionFlags.StartGo),
                startHidden: sessionFlags.HasFlag(SessionFlags.StartHidden),
                startReady: sessionFlags.HasFlag(SessionFlags.StartReady),
                startSet: sessionFlags.HasFlag(SessionFlags.StartSet),
                tenToGo: sessionFlags.HasFlag(SessionFlags.TenToGo),
                white: sessionFlags.HasFlag(SessionFlags.White),
                yellow: sessionFlags.HasFlag(SessionFlags.Yellow),
                yellowWaving: sessionFlags.HasFlag(SessionFlags.YellowWaving)
            );
        }

        public void Request(IState currentState, IRacingDataTrackerState state, IEventEnvelope envelope, IIRacingDataTracker.RequestType request)
        {
            if (request != IIRacingDataTracker.RequestType.RaceFlags)
                return;

            IRacingRaceFlags @event = GenerateEvent(currentState, envelope);
            EventBus.PublishEvent(@event);
        }
    }
}