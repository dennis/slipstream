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
    public class TowTrackerTests
    {
        private readonly TestEventBus EventBus;
        private readonly IIRacingEventFactory EventFactory;
        private readonly IRacingDataTrackerState TrackerState;
        private readonly GameStateBuilder Builder;

        public TowTrackerTests()
        {
            EventBus = new TestEventBus();
            EventFactory = new IRacingEventFactory();
            TrackerState = new IRacingDataTrackerState();
            Builder = new GameStateBuilder();
        }

        [Fact]
        public void PublishEventIfBeginTowed()
        {
            const float NOW = 1234.2f;
            const float TOW_TIME = 32.2f;

            // arrange
            Builder.AtSessionTime(NOW).Set(a => a.PlayerCarTowTime = TOW_TIME).Commit();
            Builder.AtSessionTime(NOW + 1).Set(a => a.PlayerCarTowTime = TOW_TIME - 1).Commit();
            Builder.AtSessionTime(NOW + 2).Set(a => a.PlayerCarTowTime = TOW_TIME - 2).Commit();

            // act
            var sut = new TowTracker(EventBus, EventFactory);
            foreach (var s in Builder.States)
                sut.Handle(s, TrackerState);

            // assert
            Assert.True(EventBus.Events.Count == 1);

            var @event = EventBus.Events[0] as IRacingTowed;
            Assert.NotNull(@event);
            Assert.True(@event.SessionTime == NOW);
            Assert.True(@event.RemainingTowTime == TOW_TIME);
        }

        [Fact]
        public void PublishEventsIfPlayerGotAReallyShittyRace()
        {
            const float NOW = 1234.2f;
            const float TOW_TIME = 32.2f;
            const float TOW2_TIME = 10f;

            // arrange
            // first two
            Builder.AtSessionTime(NOW).Set(a => a.PlayerCarTowTime = TOW_TIME).Commit();
            Builder.AtSessionTime(NOW + 1).Set(a => a.PlayerCarTowTime = TOW_TIME - 1).Commit();
            // all ok again
            Builder.AtSessionTime(NOW + 2).Set(a => a.PlayerCarTowTime = 0).Commit();
            // another tow
            Builder.AtSessionTime(NOW + 3).Set(a => a.PlayerCarTowTime = TOW2_TIME).Commit();

            // act
            var sut = new TowTracker(EventBus, EventFactory);
            foreach (var s in Builder.States)
                sut.Handle(s, TrackerState);

            // assert
            Assert.True(EventBus.Events.Count == 2);

            var @event = EventBus.Events[1] as IRacingTowed;
            Assert.NotNull(@event);
            Assert.True(@event.SessionTime == NOW + 3);
            Assert.True(@event.RemainingTowTime == TOW2_TIME);
        }
    }
}