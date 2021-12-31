using Slipstream.Components.IRacing.GameState;
using Slipstream.Components.IRacing.Models;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.IRacing.Trackers
{
    internal class TimeTracker : IIRacingDataTracker
    {
        private readonly IIRacingEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private double LastSessionTimeSeen = double.NaN;

        public TimeTracker(IEventBus eventBus, IIRacingEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void Handle(GameState.IState currentState, IRacingDataTrackerState state, IEventEnvelope envelope)
        {
            if (double.IsNaN(LastSessionTimeSeen) || (currentState.SessionTime - LastSessionTimeSeen) > 1.0)
            {
                LastSessionTimeSeen = currentState.SessionTime;

                EventBus.PublishEvent(EventFactory.CreateIRacingTime(
                    envelope: envelope,
                    sessionTime: currentState.SessionTime,
                    sessionTimeRemaining: currentState.SessionTimeRemaining
                ));
            }
        }

        public void Request(IState currentState, IRacingDataTrackerState state, IEventEnvelope envelope, IIRacingDataTracker.RequestType request)
        {
        }
    }
}