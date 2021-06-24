using Slipstream.Components.IRacing.Events;
using Slipstream.Components.IRacing.GameState;
using Slipstream.Components.IRacing.Models;
using Slipstream.Shared;
using static Slipstream.Components.IRacing.IIRacingEventFactory;

namespace Slipstream.Components.IRacing.Trackers
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

        public void Handle(GameState.IState currentState, IRacingDataTrackerState state, IEventEnvelope envelope)
        {
            IRacingWeatherInfo weatherInfo = GenerateEvent(currentState, envelope);

            if (state.LastWeatherInfo == null || weatherInfo.DifferentTo(state.LastWeatherInfo))
            {
                EventBus.PublishEvent(weatherInfo);

                state.LastWeatherInfo = weatherInfo;
            }
        }

        private IRacingWeatherInfo GenerateEvent(IState currentState, IEventEnvelope envelope)
        {
            return EventFactory.CreateIRacingWeatherInfo
            (
                envelope: envelope,
                sessionTime: currentState.SessionTime,
                skies: (Skies)(int)currentState.Skies,
                surfaceTemp: currentState.TrackTempCrew,
                airTemp: currentState.AirTemp,
                airPressure: currentState.AirPressure,
                relativeHumidity: currentState.RelativeHumidity,
                fogLevel: currentState.FogLevel
            );
        }

        public void Request(IState currentState, IRacingDataTrackerState state, IEventEnvelope envelope, IIRacingDataTracker.RequestType request)
        {
            if (request != IIRacingDataTracker.RequestType.WeatherInfo)
                return;

            IRacingWeatherInfo weatherInfo = GenerateEvent(currentState, envelope);
            EventBus.PublishEvent(weatherInfo);
        }
    }
}