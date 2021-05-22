using Slipstream.Components.IRacing.Models;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.IRacing.Trackers
{
    internal class TrackInfoTracker : IIRacingDataTracker
    {
        private readonly IIRacingEventFactory EventFactory;
        private readonly IEventBus EventBus;

        public TrackInfoTracker(IEventBus eventBus, IIRacingEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void Handle(GameState.IState currentState, IRacingDataTrackerState state)
        {
            if (state.SendTrackInfo)
            {
                EventBus.PublishEvent(EventFactory.CreateIRacingTrackInfo
                (
                    trackId: currentState.TrackId,
                    trackLength: currentState.TrackLength,
                    trackDisplayName: currentState.TrackDisplayName,
                    trackCity: currentState.TrackCity,
                    trackCountry: currentState.TrackCountry,
                    trackDisplayShortName: currentState.TrackDisplayShortName,
                    trackConfigName: currentState.TrackConfigName,
                    trackType: currentState.TrackType
                ));

                state.SendTrackInfo = false;
            }
        }
    }
}