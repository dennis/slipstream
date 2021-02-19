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
        private readonly TestState GameState;
        private readonly IRacingDataTrackerState TrackerState;

        public LapsCompletedTrackerTests()
        {
            EventBus = new TestEventBus();
            EventFactory = new IRacingEventFactory();
            GameState = new TestState();
            TrackerState = new IRacingDataTrackerState();
        }

        [Fact]
        public void AddUnseenCarsToTrackerState()
        {
            // arrange
            GameState.Cars = new Car[] {
                new Car {
                    Location = IIRacingEventFactory.CarLocation.NotInWorld,
                    LapsCompleted = 2
                }
            };

            // act
            var sut = new LapsCompletedTracker(EventBus, EventFactory);
            sut.Handle(GameState, TrackerState);

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
            TrackerState.Laps.Add(0, new LapState
            {
                Location = IIRacingEventFactory.CarLocation.OnTrack,
                TimingEnabled = true,
                LapsCompleted = 1,
                CurrentLapStartTime = LAP_STARTED_AT,
                FuelLevelAtLapStart = 0,
            });
            GameState.Cars = new Car[] {
                new Car {
                    Location = IIRacingEventFactory.CarLocation.OnTrack,
                    LapsCompleted = 2,
                }
            };
            GameState.SessionTime = NOW;
            GameState.DriverCarIdx = 1; // LappingLars is another drive, so we dont have fuel levels

            // act
            var sut = new LapsCompletedTracker(EventBus, EventFactory);
            sut.Handle(GameState, TrackerState);

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
        public void CarStartingFromPits()
        {
            // Car going from InPitStall to Approching pits. Which is always the location when you exit the
            // puts

            const float NOW = 40.0f;

            // arrange
            TrackerState.Laps.Add(0, new LapState
            {
                Location = IIRacingEventFactory.CarLocation.InPitStall
            });
            GameState.Cars = new Car[] {
                new Car {
                    Location = IIRacingEventFactory.CarLocation.AproachingPits,
                    LapsCompleted = 2
                }
            };
            GameState.SessionTime = NOW;

            // act
            var sut = new LapsCompletedTracker(EventBus, EventFactory);
            sut.Handle(GameState, TrackerState);

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
            TrackerState.Laps.Add(0, new LapState
            {
                Location = IIRacingEventFactory.CarLocation.OnTrack,
                TimingEnabled = true,
                PendingLapTime = true,
                CurrentLapStartTime = NOW - 3,
                LapsCompleted = 2,
                LastLapFuelDelta = -8.2f,
            });
            GameState.Cars = new Car[] {
                new Car {
                    Location = IIRacingEventFactory.CarLocation.OnTrack,
                    LapsCompleted = 2,
                    LastLapTime = 43.2f,
                }
            };
            GameState.SessionTime = NOW;

            // act
            var sut = new LapsCompletedTracker(EventBus, EventFactory);
            sut.Handle(GameState, TrackerState);

            // assert
            Assert.False(TrackerState.Laps[0].PendingLapTime);
            Assert.NotEmpty(EventBus.Events);
            Assert.True(EventBus.Events[0].EventType == "IRacingCarCompletedLap");

            var @event = EventBus.Events[0] as IRacingCarCompletedLap;

            Assert.True(@event.CarIdx == 0);
            Assert.True(@event.FuelDiff == -8.2f);
            Assert.True(@event.LapsCompleted == 2);
            Assert.True(@event.LocalUser == true);
            Assert.True(@event.SessionTime == NOW);
            Assert.True(@event.Time == 43.2f);
        }

        [Fact]
        public void CarWithPendingTimeBefore3sPublishesNoEvent()
        {
            const float NOW = 40.0f;

            // arrange
            TrackerState.Laps.Add(0, new LapState
            {
                Location = IIRacingEventFactory.CarLocation.OnTrack,
                TimingEnabled = true,
                PendingLapTime = true,
                CurrentLapStartTime = NOW - 2,
                LapsCompleted = 2,
            });
            GameState.Cars = new Car[] {
                new Car {
                    Location = IIRacingEventFactory.CarLocation.OnTrack,
                    LapsCompleted = 2,
                }
            };
            GameState.SessionTime = NOW;

            // act
            var sut = new LapsCompletedTracker(EventBus, EventFactory);
            sut.Handle(GameState, TrackerState);

            // assert
            Assert.Empty(EventBus.Events);
        }

        [Fact]
        public void CarWithPendingTimeUsesOurTimingIfOfficialLapTimeIsntAvailable()
        {
            const float NOW = 40.0f;
            const float LAP_TIME = 43.2f;

            // arrange
            TrackerState.Laps.Add(0, new LapState
            {
                Location = IIRacingEventFactory.CarLocation.OnTrack,
                TimingEnabled = true,
                PendingLapTime = true,
                CurrentLapStartTime = NOW - 3,
                LapsCompleted = 2,
                OurLapTimeMeasurement = 43.2f,
            });
            GameState.Cars = new Car[] {
                new Car {
                    Location = IIRacingEventFactory.CarLocation.OnTrack,
                    LapsCompleted = 2,
                    LastLapTime = -1,
                }
            };
            GameState.SessionTime = NOW;

            // act
            var sut = new LapsCompletedTracker(EventBus, EventFactory);
            sut.Handle(GameState, TrackerState);

            // assert
            Assert.False(TrackerState.Laps[0].PendingLapTime);
            Assert.NotEmpty(EventBus.Events);
            Assert.True(EventBus.Events[0].EventType == "IRacingCarCompletedLap");

            var @event = EventBus.Events[0] as IRacingCarCompletedLap;

            Assert.True(@event.Time == LAP_TIME);
        }

        [Fact]
        public void DriverDoesANewLapWithoutTimingEnabled()
        {
            const float LAP_STARTED_AT = 20.0f;
            const float NOW = 40.0f;
            const float FUEL_AT_START_LAP = 100f;
            const float FUEL_NOW = 92.2f;
            // arrange
            TrackerState.Laps.Add(0, new LapState
            {
                Location = IIRacingEventFactory.CarLocation.OnTrack,
                TimingEnabled = false,
                LapsCompleted = 1,
                CurrentLapStartTime = LAP_STARTED_AT,
                FuelLevelAtLapStart = FUEL_AT_START_LAP,
            });
            GameState.Cars = new Car[] {
                new Car {
                    Location = IIRacingEventFactory.CarLocation.OnTrack,
                    LapsCompleted = 2,
                }
            };
            GameState.SessionTime = NOW;
            GameState.FuelLevel = FUEL_NOW;
            GameState.DriverCarIdx = 0;

            // act
            var sut = new LapsCompletedTracker(EventBus, EventFactory);
            sut.Handle(GameState, TrackerState);

            // assert
            Assert.True(TrackerState.Laps[0].OurLapTimeMeasurement == NOW - LAP_STARTED_AT);
            Assert.True(TrackerState.Laps[0].CurrentLapStartTime == NOW);
            Assert.False(TrackerState.Laps[0].PendingLapTime);
            Assert.True(TrackerState.Laps[0].TimingEnabled); // Make sure we time the next lap
            Assert.True(TrackerState.Laps[0].FuelLevelAtLapStart == FUEL_NOW);
            Assert.True(TrackerState.Laps[0].LastLapFuelDelta == FUEL_NOW - FUEL_AT_START_LAP);
            Assert.True(TrackerState.Laps[0].LapsCompleted == 2);
            Assert.Empty(EventBus.Events);
        }

        [Fact]
        public void DriverDoesANewLapWithTimingEnabled()
        {
            const float LAP_STATED_AT = 20.0f;
            const float NOW = 40.0f;
            const float FUEL_AT_LAP_START = 100f;
            const float FUEL_NOW = 92.2f;
            // arrange
            TrackerState.Laps.Add(0, new LapState
            {
                Location = IIRacingEventFactory.CarLocation.OnTrack,
                TimingEnabled = true,
                LapsCompleted = 1,
                CurrentLapStartTime = LAP_STATED_AT,
                FuelLevelAtLapStart = FUEL_AT_LAP_START,
            });
            GameState.Cars = new Car[] {
                new Car {
                    Location = IIRacingEventFactory.CarLocation.OnTrack,
                    LapsCompleted = 2,
                }
            };
            GameState.SessionTime = NOW;
            GameState.FuelLevel = FUEL_NOW;
            GameState.DriverCarIdx = 0;

            // act
            var sut = new LapsCompletedTracker(EventBus, EventFactory);
            sut.Handle(GameState, TrackerState);

            // assert
            Assert.True(TrackerState.Laps[0].OurLapTimeMeasurement == NOW - LAP_STATED_AT);
            Assert.True(TrackerState.Laps[0].CurrentLapStartTime == NOW);
            Assert.True(TrackerState.Laps[0].PendingLapTime);
            Assert.True(TrackerState.Laps[0].TimingEnabled); // Make sure we time the next lap
            Assert.True(TrackerState.Laps[0].FuelLevelAtLapStart == FUEL_NOW);
            Assert.True(TrackerState.Laps[0].LastLapFuelDelta == FUEL_NOW - FUEL_AT_LAP_START);
            Assert.True(TrackerState.Laps[0].LapsCompleted == 2);
            Assert.Empty(EventBus.Events);
        }

        [Fact]
        public void StopTimingWhenResettingToPits()
        {
            // arrange
            TrackerState.Laps.Add(0, new LapState
            {
                Location = IIRacingEventFactory.CarLocation.OnTrack,
                TimingEnabled = true,
            });
            GameState.Cars = new Car[] {
                new Car {
                    Location = IIRacingEventFactory.CarLocation.NotInWorld,
                    LapsCompleted = 2
                }
            };

            // act
            var sut = new LapsCompletedTracker(EventBus, EventFactory);
            sut.Handle(GameState, TrackerState);

            // assert
            Assert.False(TrackerState.Laps[0].TimingEnabled);
            Assert.True(TrackerState.Laps[0].Location == IIRacingEventFactory.CarLocation.NotInWorld);
        }
    }
}