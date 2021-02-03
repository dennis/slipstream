using iRacingSDK;
using Slipstream.Components.IRacing.Plugins.Models;
using Slipstream.Shared;

namespace Slipstream.Components.IRacing.Plugins.Trackers
{
    internal class WeatherTracker : IIRacingDataTracker
    {
        private readonly IIRacingEventFactory EventFactory;
        private readonly IEventBus EventBus;

        public WeatherTracker(IEventBus eventBus, IIRacingEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void Handle(DataSample data, IRacingDataTrackerState state)
        {
            var weatherInfo = EventFactory.CreateIRacingWeatherInfo
            (
                sessionTime: data.Telemetry.SessionTime,
                skies: data.SessionData.WeekendInfo.TrackSkies,
                surfaceTemp: data.SessionData.WeekendInfo.TrackSurfaceTemp,
                airTemp: data.SessionData.WeekendInfo.TrackAirTemp,
                airPressure: data.SessionData.WeekendInfo.TrackAirPressure,
                relativeHumidity: data.SessionData.WeekendInfo.TrackRelativeHumidity,
                fogLevel: data.SessionData.WeekendInfo.TrackFogLevel
            );

            if (state.LastWeatherInfo == null || !weatherInfo.DifferentTo(state.LastWeatherInfo) || state.SendWeatherInfo)
            {
                EventBus.PublishEvent(weatherInfo);

                state.LastWeatherInfo = weatherInfo;
                state.SendWeatherInfo = false;
            }
        }
    }
}