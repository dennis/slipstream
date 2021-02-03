using iRacingSDK;
using Slipstream.Components.IRacing.Plugins.Models;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.IRacing.Plugins.Trackers
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

        public void Handle(DataSample data, IRacingDataTrackerState state)
        {
            if (state.SendTrackInfo)
            {
                EventBus.PublishEvent(EventFactory.CreateIRacingTrackInfo
                (
                    trackId: data.SessionData.WeekendInfo.TrackID,
                    trackLength: data.SessionData.WeekendInfo.TrackLength,
                    trackDisplayName: data.SessionData.WeekendInfo.TrackDisplayName,
                    trackCity: data.SessionData.WeekendInfo.TrackCity,
                    trackCountry: data.SessionData.WeekendInfo.TrackCountry,
                    trackDisplayShortName: data.SessionData.WeekendInfo.TrackDisplayShortName,
                    trackConfigName: data.SessionData.WeekendInfo.TrackConfigName,
                    trackType: data.SessionData.WeekendInfo.TrackType
                ));

                state.SendTrackInfo = false;
            }
        }
    }
}