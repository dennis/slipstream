using Slipstream.Components.IRacing;
using Slipstream.Components.IRacing.EventFactory;
using Slipstream.Components.IRacing.Events;
using Slipstream.Components.IRacing.GameState;
using Slipstream.Components.IRacing.Models;
using Slipstream.Components.IRacing.Trackers;
using Slipstream.UnitTests.Components.IRacing.Trackers;
using Slipstream.UnitTests.TestData;
using Xunit;

namespace Slipstream.UnitTests.Components.IRacing.Plugins.Trackers
{
    public class IncidentTrackerTests
    {
        private readonly TestEventBus EventBus;
        private readonly IIRacingEventFactory EventFactory;
        private readonly IRacingDataTrackerState TrackerState;
        private readonly GameStateBuilder Builder;

        public IncidentTrackerTests()
        {
            EventBus = new TestEventBus();
            EventFactory = new IRacingEventFactory();
            TrackerState = new IRacingDataTrackerState();
            Builder = new GameStateBuilder();
        }

        [Fact]
        public void PublishEventWhenMyIncidentCountIncreases()
        {
            // arrange
            Builder.Set(a => a.MyIncidentCount = 1).Commit();
            Builder.Set(a => a.MyIncidentCount = 3).Commit();

            // act
            var sut = new IncidentTracker(EventBus, EventFactory);
            foreach (var s in Builder.States)
                sut.Handle(s, TrackerState);

            // assert
            Assert.True(EventBus.Events.Count == 2);

            var event1 = EventBus.Events[0] as IRacingDriverIncident;
            Assert.NotNull(event1);
            Assert.True(event1.MyIncidentCount == 1);
            Assert.True(event1.MyIncidentDelta == 1);

            var event2 = EventBus.Events[1] as IRacingDriverIncident;
            Assert.NotNull(event2);
            Assert.True(event2.MyIncidentCount == 3);
            Assert.True(event2.MyIncidentDelta == 2);
        }

        [Fact]
        public void PublishEventWhenDriverIncidentCountIncreases()
        {
            // arrange
            Builder.Set(a => a.DriverIncidentCount = 1).Commit();
            Builder.Set(a => a.DriverIncidentCount = 3).Commit();

            // act
            var sut = new IncidentTracker(EventBus, EventFactory);
            foreach (var s in Builder.States)
                sut.Handle(s, TrackerState);

            // assert
            Assert.True(EventBus.Events.Count == 2);

            var event1 = EventBus.Events[0] as IRacingDriverIncident;
            Assert.NotNull(event1);
            Assert.True(event1.DriverIncidentCount == 1);
            Assert.True(event1.DriverIncidentDelta == 1);

            var event2 = EventBus.Events[1] as IRacingDriverIncident;
            Assert.NotNull(event2);
            Assert.True(event2.DriverIncidentCount == 3);
            Assert.True(event2.DriverIncidentDelta == 2);
        }

        [Fact]
        public void PublishEventWhenTeamIncidentCountIncreases()
        {
            // arrange
            Builder.Set(a => a.TeamIncidentCount = 1).Commit();
            Builder.Set(a => a.TeamIncidentCount = 3).Commit();

            // act
            var sut = new IncidentTracker(EventBus, EventFactory);
            foreach (var s in Builder.States)
                sut.Handle(s, TrackerState);

            // assert
            Assert.True(EventBus.Events.Count == 2);

            var event1 = EventBus.Events[0] as IRacingDriverIncident;
            Assert.NotNull(event1);
            Assert.True(event1.TeamIncidentCount == 1);
            Assert.True(event1.TeamIncidentDelta == 1);

            var event2 = EventBus.Events[1] as IRacingDriverIncident;
            Assert.NotNull(event2);
            Assert.True(event2.TeamIncidentCount == 3);
            Assert.True(event2.TeamIncidentDelta == 2);
        }
    }
}