using Slipstream.Components.IRacing.Plugins.Models;
using Slipstream.Shared;
using static Slipstream.Components.IRacing.IIRacingEventFactory;

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

        public void Handle(GameState.IState currentState, IRacingDataTrackerState state)
        {
            var weatherInfo = EventFactory.CreateIRacingWeatherInfo
            (
                sessionTime: currentState.SessionTime,
                skies: (Skies)(int)currentState.Skies,
                surfaceTemp: currentState.TrackTempCrew,
                airTemp: currentState.AirTemp,
                airPressure: currentState.AirPressure,
                relativeHumidity: currentState.RelativeHumidity,
                fogLevel: currentState.FogLevel
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