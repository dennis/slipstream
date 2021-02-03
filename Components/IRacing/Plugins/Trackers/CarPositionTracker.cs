using iRacingSDK;
using Slipstream.Components.IRacing.Plugins.Models;
using Slipstream.Shared;
using System.Linq;

#nullable enable

namespace Slipstream.Components.IRacing.Plugins.Trackers
{
    internal class CarPositionTracker : IIRacingDataTracker
    {
        private readonly IIRacingEventFactory EventFactory;
        private readonly IEventBus EventBus;

        public CarPositionTracker(IEventBus eventBus, IIRacingEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void Handle(DataSample data, IRacingDataTrackerState state)
        {
            for (int i = 0; i < data.Telemetry.Cars.Count(); i++)
            {
                int positionInRace = (int)data.Telemetry.CarIdxClassPosition[i];
                int positionInClass = (int)data.Telemetry.CarIdxPosition[i];

                if (positionInRace > 0 && positionInClass > 0)
                {
                    if (positionInRace != state.LastPositionInRace[i] || positionInClass != state.LastPositionInClass[i])
                    {
                        var localUser = data.SessionData.DriverInfo.DriverCarIdx == i;

                        var @event = EventFactory.CreateIRacingCarPosition(
                            sessionTime: data.Telemetry.SessionTime,
                            carIdx: i,
                            localUser: localUser,
                            positionInClass: positionInClass,
                            positionInRace: positionInRace);

                        EventBus.PublishEvent(@event);
                    }
                }

                state.LastPositionInClass[i] = positionInClass;
                state.LastPositionInRace[i] = positionInRace;
            }
        }
    }
}