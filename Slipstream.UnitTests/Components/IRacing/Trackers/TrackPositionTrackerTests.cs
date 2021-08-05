using Slipstream.Components.IRacing;
using Slipstream.Components.IRacing.EventFactory;
using Slipstream.Components.IRacing.Events;
using Slipstream.Components.IRacing.Models;
using Slipstream.Components.IRacing.Trackers;
using System.Linq;
using Slipstream.UnitTests.TestData;
using Xunit;
using Slipstream.UnitTests.Components.IRacing.Trackers;
using Slipstream.Shared;

namespace Slipstream.UnitTests.Components.IRacing.Plugins.Trackers
{
    public class TrackPositionTrackerTest
    {
        private readonly TestEventBus EventBus;
        private readonly IIRacingEventFactory EventFactory;
        private readonly IRacingDataTrackerState TrackerState;
        private readonly GameStateBuilder Builder;

        public TrackPositionTrackerTest()
        {
            EventBus = new TestEventBus();
            EventFactory = new IRacingEventFactory();
            TrackerState = new IRacingDataTrackerState();
            Builder = new GameStateBuilder();
        }

        [Fact]
        public void CarOvertakesOtherCarInDifferentClass()
        {
            const float NOW = 1234.2f;

            // arrange

            // car0 is front of car1
            Builder
                .AtSessionTime(NOW)
                .Set(a => a.DriverCarIdx = 0)
                .Set(a => a.SessionType = IIRacingEventFactory.IRacingSessionTypeEnum.Race)
                .Set(a => a.SessionState = IIRacingEventFactory.IRacingSessionStateEnum.Racing)
                .Car(0).OnTrack().Set(a => a.CarClassId = 1).Set(a => a.LapDistPct = 0.44f).CarDone()
                .Car(1).OnTrack().Set(a => a.CarClassId = 2).Set(a => a.LapDistPct = 0.40f).CarDone()
                .Commit();
            // car1 overtakes car0
            Builder
                .AtSessionTime(NOW + 1)
                .Car(0).OnTrack().Set(a => a.LapDistPct = 0.44f).CarDone()
                .Car(1).OnTrack().Set(a => a.LapDistPct = 0.50f).CarDone()
                .Commit();

            // act
            var sut = new TrackPositionTracker(EventBus, EventFactory);
            foreach (var s in Builder.States)
                sut.Handle(s, TrackerState, new EventEnvelope("sender"));

            // assert
            Assert.True(EventBus.Events.Count == 2);

            foreach (var rawEvent in EventBus.Events)
            {
                var @event = rawEvent as IRacingTrackPosition;

                Assert.NotNull(@event);
                Assert.True(@event.SessionTime == NOW + 1);

                // class-wise, there isn't any change, as the two cars are in
                // different class.

                if (@event.CarIdx == 0)
                {
                    Assert.True(@event.LocalUser);
                    Assert.True(@event.PreviousPositionInRace == 1);
                    Assert.True(@event.CurrentPositionInRace == 2);
                    Assert.True(@event.PreviousPositionInClass == 1);
                    Assert.True(@event.CurrentPositionInClass == 1);
                    Assert.True(@event.NewCarsAhead.SequenceEqual(new int[] { 1 }));
                    Assert.True(@event.NewCarsBehind.Length == 0);
                }
                else if (@event.CarIdx == 1)
                {
                    Assert.False(@event.LocalUser);
                    Assert.True(@event.PreviousPositionInRace == 2);
                    Assert.True(@event.CurrentPositionInRace == 1);
                    Assert.True(@event.PreviousPositionInClass == 1);
                    Assert.True(@event.CurrentPositionInClass == 1);
                    Assert.True(@event.NewCarsAhead.Length == 0);
                    Assert.True(@event.NewCarsBehind.SequenceEqual(new int[] { 0 }));
                }
                else
                {
                    Assert.Null("Unexpected caridx");
                }
            }
        }

        [Fact]
        public void NewCarMidSession()
        {
            const float NOW = 1234.2f;

            // arrange

            // car0 is front of car1
            Builder
                .AtSessionTime(NOW)
                .Set(a => a.DriverCarIdx = 0)
                .Set(a => a.SessionType = IIRacingEventFactory.IRacingSessionTypeEnum.Race)
                .Set(a => a.SessionState = IIRacingEventFactory.IRacingSessionStateEnum.Racing)
                .Car(0).OnTrack().Set(a => a.LapDistPct = 0.44f).CarDone()
                .Commit();
            // wild car1 appears
            Builder
                .AtSessionTime(NOW + 1)
                .Car(1).OnTrack().Set(a => a.LapDistPct = 0.50f).CarDone()
                .Commit();

            // act
            var sut = new TrackPositionTracker(EventBus, EventFactory);
            foreach (var s in Builder.States)
                sut.Handle(s, TrackerState, new EventEnvelope("sender"));

            // assert
            Assert.Single(EventBus.Events);
        }
    }
}