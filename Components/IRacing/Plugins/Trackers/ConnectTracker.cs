using iRacingSDK;
using Slipstream.Components.IRacing.Plugins.Models;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.IRacing.Plugins.Trackers
{
    internal class ConnectTracker : IIRacingDataTracker
    {
        private readonly IIRacingEventFactory EventFactory;
        private readonly IEventBus EventBus;

        public ConnectTracker(IEventBus eventBus, IIRacingEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void Handle(DataSample data, IRacingDataTrackerState state)
        {
            if (!state.Connected)
            {
                state.FullReset();

                EventBus.PublishEvent(EventFactory.CreateIRacingConnected());

                state.Connected = true;
                state.SendTrackInfo = true;
            }

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