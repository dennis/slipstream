#nullable enable

using Slipstream.Components.IRacing.Models;
using Slipstream.Shared;

using System.Collections.Generic;

using static Slipstream.Components.IRacing.Trackers.IIRacingDataTracker;

namespace Slipstream.Components.IRacing.Trackers
{
    internal class Trackers
    {
        private readonly IRacingDataTrackerState TrackerState = new IRacingDataTrackerState();
        private readonly List<IIRacingDataTracker> DataTrackers = new List<IIRacingDataTracker>();

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
            DataTrackers.Add(new TowTracker(eventBus, eventFactory));
            DataTrackers.Add(new TrackPositionTracker(eventBus, eventFactory));
            DataTrackers.Add(new TimeTracker(eventBus, eventFactory));
        }

        public void Handle(GameState.IState currentState, IEventEnvelope envelope)
        {
            foreach (var t in DataTrackers)
                t.Handle(currentState, TrackerState, envelope);
        }

        public void Request(GameState.IState? currentState, IEventEnvelope envelope, RequestType type)
        {
            if (currentState == null)
                return;

            foreach (var t in DataTrackers)
                t.Request(currentState, TrackerState, envelope, type);
        }
    }
}