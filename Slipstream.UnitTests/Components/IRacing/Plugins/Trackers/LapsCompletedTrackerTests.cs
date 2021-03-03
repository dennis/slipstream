using Slipstream.Components.IRacing;
using Slipstream.Components.IRacing.EventFactory;
using Slipstream.Components.IRacing.Events;
using Slipstream.Components.IRacing.Plugins.GameState;
using Slipstream.Components.IRacing.Plugins.Models;
using Slipstream.Components.IRacing.Plugins.Trackers;
using Slipstream.UnitTests.TestData;
using Xunit;

namespace Slipstream.UnitTests.Components.IRacing.Plugins.Trackers
{
    public class LapsCompletedTrackerTests
    {
        private readonly TestEventBus EventBus;
        private readonly IIRacingEventFactory EventFactory;
        private readonly IRacingDataTrackerState TrackerState;
        private readonly GameStateBuilder Builder;

        public LapsCompletedTrackerTests()
        {
            EventBus = new TestEventBus();
            EventFactory = new IRacingEventFactory();
            TrackerState = new IRacingDataTrackerState();
            Builder = new GameStateBuilder();
        }

        [Fact]
        public void AddUnseenCarsToTrackerState()
        {
            // arrange
            Builder.Car(0).EntersGame().AtLap(2).Commit();

            // act
            var sut = new LapsCompletedTracker(EventBus, EventFactory);
            foreach (var s in Builder.States)
                sut.Handle(s, TrackerState);

            // assert
            Assert.True(TrackerState.Laps.Count == 1);
            Assert.False(TrackerState.Laps[0].TimingEnabled);
            Assert.True(TrackerState.Laps[0].Location == IIRacingEventFactory.CarLocation.NotInWorld);
            Assert.True(TrackerState.Laps[0].LapsCompleted == 2);
            Assert.Empty(EventBus.Events);
        }

        [Fact]
        public void CarDoesANewLapWithTimingEnabled()
        {
            const float LAP_STARTED_AT = 20.0f;
            const float NOW = 40.0f;

            // arrange
            Builder.ChangeState(a => a.DriverCarIdx = 1); // This is not our car, so we wont have fuel data
            Builder.Car(0).EntersGame().AtLap(1).Commit();
            Builder.AtSessionTime(LAP_STARTED_AT).Car(0).InPits().Commit();
            Builder.Car(0).ExitingPits().Commit();
            Builder.AtSessionTime(NOW).Car(0).LapsCompleted(2).Commit();

            // act
            var sut = new LapsCompletedTracker(EventBus, EventFactory);
            foreach (var s in Builder.States)
                sut.Handle(s, TrackerState);

            // assert
            Assert.True(TrackerState.Laps[0].OurLapTimeMeasurement == NOW - LAP_STARTED_AT);
            Assert.True(TrackerState.Laps[0].CurrentLapStartTime == NOW);
            Assert.True(TrackerState.Laps[0].PendingLapTime);
            Assert.True(TrackerState.Laps[0].TimingEnabled); // Make sure we time the next lap
            Assert.True(TrackerState.Laps[0].FuelLevelAtLapStart == 0);
            Assert.Null(TrackerState.Laps[0].LastLapFuelDelta);
            Assert.True(TrackerState.Laps[0].LapsCompleted == 2);
            Assert.Empty(EventBus.Events);
        }

        [Fact]
        public void CarDoesInitialLap()
        {
            const float LAP_STARTED_AT = 20.0f;
            const float NOW = 40.0f;

            // arrange
            Builder.Car(0).EntersGame().Commit();
            Builder.AtSessionTime(LAP_STARTED_AT).Car(0).InPits().Commit();
            Builder.Car(0).ExitingPits().Commit();
            Builder.AtSessionTime(NOW).Car(0).LapsCompleted(1).Commit();

            // act
            var sut = new LapsCompletedTracker(EventBus, EventFactory);
            foreach (var s in Builder.States)
                sut.Handle(s, TrackerState);

            // assert
            Assert.False(TrackerState.Laps[0].PendingLapTime);
            Assert.True(TrackerState.Laps[0].TimingEnabled);
            Assert.Empty(EventBus.Events);
        }

        [Fact]
        public void CarStartingFromPits()
        {
            // Car going from InPitStall to Approching pits. Which is always the location when you exit the
            // puts

            const float NOW = 40.0f;

            // arrange
            Builder.Car(0).EntersGame().Commit();
            Builder.AtSessionTime(NOW).Car(0).InPits().Commit();
            Builder.Car(0).LapsCompleted(2).Commit();
            Builder.Car(0).ExitingPits().Commit();

            // act
            var sut = new LapsCompletedTracker(EventBus, EventFactory);
            foreach (var s in Builder.States)
                sut.Handle(s, TrackerState);

            // assert
            Assert.True(TrackerState.Laps.Count == 1);
            Assert.True(TrackerState.Laps.ContainsKey(0));
            Assert.True(TrackerState.Laps[0].TimingEnabled);
            Assert.True(TrackerState.Laps[0].CurrentLapStartTime == NOW); // Current SessionTime
            Assert.True(TrackerState.Laps[0].Location == IIRacingEventFactory.CarLocation.AproachingPits);
            Assert.Empty(EventBus.Events);
        }

        [Fact]
        public void CarWithPendingTimeAfter3sPublishesEvent()
        {
            const float NOW = 40.0f;

            // arrange
            Builder.Car(0).LapsCompleted(0).Commit();
            Builder.FuelLevel(80.0f).Car(0).OnTrack().LapsCompleted(1).Commit();
            Builder.AtSessionTime(NOW - 3).FuelLevel(69.0f).Car(0).LapsCompleted(2).Commit();
            Builder.AtSessionTime(NOW).FuelLevel(59.0f).Car(0).LastLapTime(43.2f).Commit();

            // act
            var sut = new LapsCompletedTracker(EventBus, EventFactory);
            foreach (var s in Builder.States)
                sut.Handle(s, TrackerState);

            // assert
            Assert.False(TrackerState.Laps[0].PendingLapTime);
            Assert.NotEmpty(EventBus.Events);
            Assert.True(EventBus.Events[0].EventType == "IRacingCompletedLap");

            var @event = EventBus.Events[0] as IRacingCompletedLap;

            Assert.True(@event.CarIdx == 0);
            Assert.True(@event.FuelDelta == -11.0f);
            Assert.True(@event.LapsCompleted == 2);
            Assert.True(@event.LocalUser == true);
            Assert.True(@event.SessionTime == NOW);
            Assert.True(@event.Time == 43.2f);
            Assert.False(@event.BestLap);
        }

        [Fact]
        public void CarWithPendingTimeBefore3sPublishesNoEvent()
        {
            const float NOW = 40.0f;

            // arrange
            Builder.Car(0).LapsCompleted(0).Commit();
            Builder.FuelLevel(80.0f).Car(0).OnTrack().LapsCompleted(1).Commit();
            Builder.AtSessionTime(NOW - 2).Car(0).LapsCompleted(2).Commit();
            Builder.AtSessionTime(NOW).Car(0).LastLapTime(43.2f).Commit();

            // act
            var sut = new LapsCompletedTracker(EventBus, EventFactory);
            foreach (var s in Builder.States)
                sut.Handle(s, TrackerState);

            // assert
            Assert.Empty(EventBus.Events);
        }

        [Fact]
        public void CarWithPendingTimeUsesOurTimingIfOfficialLapTimeIsntAvailable()
        {
            const float NOW = 50.0f;
            const float LAP_TIME = 43.2f;

            // arrange
            Builder.Car(0).LapsCompleted(0).Commit();
            Builder.FuelLevel(80.0f).Car(0).OnTrack().LapsCompleted(1).Commit();
            Builder.AtSessionTime(LAP_TIME).Car(0).LapsCompleted(2).Commit();
            Builder.AtSessionTime(NOW).Car(0).LastLapTime(-1).Commit();

            // act
            var sut = new LapsCompletedTracker(EventBus, EventFactory);
            foreach (var s in Builder.States)
                sut.Handle(s, TrackerState);

            // assert
            Assert.False(TrackerState.Laps[0].PendingLapTime);
            Assert.NotEmpty(EventBus.Events);
            Assert.True(EventBus.Events[0].EventType == "IRacingCompletedLap");

            var @event = EventBus.Events[0] as IRacingCompletedLap;

            Assert.True(@event.Time == LAP_TIME);
        }

        [Fact]
        public void StopTimingWhenResettingToPits()
        {
            // arrange
            Builder.AtSessionTime(0).Car(0).InPits().Commit();
            Builder.Car(0).ExitingPits().Commit();
            // We reset: multiple NotInWorld locations will be received until you're at the pits again
            Builder.AtSessionTime(2.0f).Car(0).NotInWorld().Commit();
            Builder.AtSessionTime(2.1f).Commit();
            Builder.AtSessionTime(2.2f).Commit();

            // act
            var sut = new LapsCompletedTracker(EventBus, EventFactory);
            foreach (var s in Builder.States)
                sut.Handle(s, TrackerState);

            // assert
            Assert.False(TrackerState.Laps[0].TimingEnabled);
            Assert.True(TrackerState.Laps[0].Location == IIRacingEventFactory.CarLocation.NotInWorld);
            Assert.Empty(EventBus.Events);
        }

        [Fact]
        public void IgnoreSingleNotInWorldAsThisMightJustBeDroppedFrames()
        {
            // arrange
            Builder.AtSessionTime(0).Car(0).InPits().Commit();
            Builder.Car(0).ExitingPits().Commit();
            Builder.AtSessionTime(2.0f).Car(0).NotInWorld().Commit();

            // act
            var sut = new LapsCompletedTracker(EventBus, EventFactory);
            foreach (var s in Builder.States)
                sut.Handle(s, TrackerState);

            // assert
            Assert.True(TrackerState.Laps[0].TimingEnabled);
            Assert.True(TrackerState.Laps[0].Location == IIRacingEventFactory.CarLocation.NotInWorld);
            Assert.True(TrackerState.Laps[0].ConsecutiveNotInWorld == 1);
        }

        [Fact]
        public void ResetAtSessionChange()
        {
            const float NOW = 40.0f;

            // arrange
            Builder.Car(0).LapsCompleted(0).Commit();
            Builder.Car(0).OnTrack().LapsCompleted(1).Commit();
            Builder.AtSessionTime(NOW - 3).Car(0).LapsCompleted(2).Commit();
            Builder.AtSessionTime(NOW).Car(0).LastLapTime(43.2f).Commit();
            Builder.InSessionNum(1).Car(0).LapsCompleted(-1).Commit();

            // act
            var sut = new LapsCompletedTracker(EventBus, EventFactory);
            foreach (var s in Builder.States)
                sut.Handle(s, TrackerState);

            // assert
            Assert.True(TrackerState.Laps[0].OurLapTimeMeasurement == 0);
            Assert.True(TrackerState.Laps[0].CurrentLapStartTime == 0);
            Assert.False(TrackerState.Laps[0].PendingLapTime);
            Assert.False(TrackerState.Laps[0].TimingEnabled);
            Assert.True(TrackerState.Laps[0].FuelLevelAtLapStart == 0);
            Assert.True(TrackerState.Laps[0].LastLapFuelDelta == 0);
            Assert.True(TrackerState.Laps[0].LapsCompleted == -1);
        }

        [Fact]
        public void CarSettingFastestLap()
        {
            const float NOW = 40.0f;

            // arrange
            Builder.Car(0).LapsCompleted(0).Commit();
            Builder.Car(0).OnTrack().LapsCompleted(1).Commit();
            Builder.AtSessionTime(NOW - 3).Car(0).LapsCompleted(2).Commit();
            Builder.AtSessionTime(NOW).Car(0).LastLapTime(43.2f).BestLapNum(2).Commit();

            // act
            var sut = new LapsCompletedTracker(EventBus, EventFactory);
            foreach (var s in Builder.States)
                sut.Handle(s, TrackerState);

            // assert
            Assert.False(TrackerState.Laps[0].PendingLapTime);
            Assert.NotEmpty(EventBus.Events);
            Assert.True(EventBus.Events[0].EventType == "IRacingCompletedLap");

            var @event = EventBus.Events[0] as IRacingCompletedLap;

            Assert.NotNull(@event);
            Assert.True(@event.BestLap);
        }
    }
}