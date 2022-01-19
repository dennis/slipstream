using Slipstream.Components.IRacing.Models;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.IRacing.Trackers
{
    internal interface IIRacingDataTracker
    {
        public enum RequestType
        {
            CarInfo,
            TrackInfo,
            WeatherInfo,
            SessionState,
            RaceFlags
        }

        public void Handle(GameState.IState currentState, IRacingDataTrackerState state, IEventEnvelope envelope);
        public void Request(GameState.IState currentState, IRacingDataTrackerState state, IEventEnvelope envelope, RequestType request);
    }
}