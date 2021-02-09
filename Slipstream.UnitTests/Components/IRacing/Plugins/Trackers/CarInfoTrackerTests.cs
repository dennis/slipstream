using Moq;
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
    public class CarInfoTrackerTests
    {
        [Fact]
        public void CanInstantiateCarInfoTracker()
        {
            // arrange
            var eventBusMock = new Mock<Shared.IEventBus>();
            var eventFactoryMock = new Mock<IIRacingEventFactory>();

            // act
            var sut = new CarInfoTracker(eventBusMock.Object, eventFactoryMock.Object);

            // assert
            Assert.NotNull(sut);
        }

        [Theory]
        [ClassData(typeof(CarInfoDataSampleTestData))]
        internal void HandleRaisesIRacingCarInfo_ForAllCars(IState currentState)
        {
            // arrange
            var eventBusMock = new Mock<Shared.IEventBus>();
            var eventFactory = new IRacingEventFactory();
            var trackerDataState = new IRacingDataTrackerState();
            var sut = new CarInfoTracker(eventBusMock.Object, eventFactory);

            // act
            sut.Handle(currentState, trackerDataState);

            // assert
            eventBusMock.Verify(eb => eb.PublishEvent(It.IsAny<IRacingCarInfo>()), Times.Once, "IRacingCarInfo event not raised");
        }
    }
}