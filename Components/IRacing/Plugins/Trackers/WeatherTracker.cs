using iRacingSDK;
using Slipstream.Components.IRacing.Plugins.Models;
using Slipstream.Shared;
using System;

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
                skies: data.Telemetry.Skies,
                surfaceTemp: data.Telemetry.TrackTempCrew,
                airTemp: data.Telemetry.AirTemp,
                airPressure: data.Telemetry.AirPressure,
                relativeHumidity: data.Telemetry.RelativeHumidity,
                fogLevel: data.Telemetry.FogLevel
            );

            if (state.LastWeatherInfo == null || weatherInfo.DifferentTo(state.LastWeatherInfo) || state.SendWeatherInfo)
            {
                EventBus.PublishEvent(weatherInfo);

                state.LastWeatherInfo = weatherInfo;
                state.SendWeatherInfo = false;
            }
        }
    }
}