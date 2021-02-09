using Slipstream.Components.IRacing.Plugins.Models;
using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.IRacing.Plugins.Trackers
{
    internal class Trackers
    {
        private readonly IRacingDataTrackerState TrackerState = new IRacingDataTrackerState();
        private readonly List<IIRacingDataTracker> DataTrackers = new List<IIRacingDataTracker>();

        public bool SendCarInfo { get => TrackerState.SendCarInfo; set => TrackerState.SendCarInfo = value; }
        public bool SendTrackInfo { get => TrackerState.SendTrackInfo; set => TrackerState.SendTrackInfo = value; }
        public bool SendWeatherInfo { get => TrackerState.SendWeatherInfo; set => TrackerState.SendWeatherInfo = value; }
        public bool SendSessionState { get => TrackerState.SendSessionState; set => TrackerState.SendSessionState = value; }
        public bool SendRaceFlags { get => TrackerState.SendRaceFlags; set => TrackerState.SendRaceFlags = value; }
        public bool Connected { get => TrackerState.Connected; set => TrackerState.Connected = value; }

        public Trackers(IEventBus eventBus, IIRacingEventFactory eventFactory)
        {
            DataTrackers.Add(new ConnectTracker(eventBus, eventFactory));
            DataTrackers.Add(new TrackInfoTracker(eventBus, eventFactory));
            DataTrackers.Add(new WeatherTracker(eventBus, eventFactory));
            DataTrackers.Add(new CarInfoTracker(eventBus, eventFactory));
            DataTrackers.Add(new IncidentTracker(eventBus, eventFactory));
            DataTrackers.Add(new FlagsTracker(eventBus, eventFactory));
            DataTrackers.Add(new LapsCompletedTracker(eventBus, eventFactory));
            DataTrackers.Add(new PitUsageTracker(eventBus, eventFactory));
            DataTrackers.Add(new IRacingSessionTracker(eventBus, eventFactory));
            DataTrackers.Add(new CarPositionTracker(eventBus, eventFactory));
        }

        public void Handle(GameState.IState currentState)
        {
            foreach (var t in DataTrackers)
                t.Handle(currentState, TrackerState);
        }
    }
}