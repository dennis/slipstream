using iRacingSDK;
using Slipstream.Components.IRacing.Plugins.Models;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.IRacing.Plugins.Trackers
{
    internal class FlagsTracker : IIRacingDataTracker
    {
        private readonly IIRacingEventFactory EventFactory;
        private readonly IEventBus EventBus;

        public FlagsTracker(IEventBus eventBus, IIRacingEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void Handle(DataSample data, IRacingDataTrackerState state)
        {
            var sessionFlags = data.Telemetry.SessionFlags;

            var @event = EventFactory.CreateIRacingRaceFlags
            (
                sessionTime: data.Telemetry.SessionTime,
                black: sessionFlags.HasFlag(SessionFlags.black),
                blue: sessionFlags.HasFlag(SessionFlags.blue),
                caution: sessionFlags.HasFlag(SessionFlags.caution),
                cautionWaving: sessionFlags.HasFlag(SessionFlags.cautionWaving),
                checkered: sessionFlags.HasFlag(SessionFlags.checkered),
                crossed: sessionFlags.HasFlag(SessionFlags.crossed),
                debris: sessionFlags.HasFlag(SessionFlags.debris),
                disqualify: sessionFlags.HasFlag(SessionFlags.disqualify),
                fiveToGo: sessionFlags.HasFlag(SessionFlags.fiveToGo),
                furled: sessionFlags.HasFlag(SessionFlags.furled),
                green: sessionFlags.HasFlag(SessionFlags.green),
                greenHeld: sessionFlags.HasFlag(SessionFlags.greenHeld),
                oneLapToGreen: sessionFlags.HasFlag(SessionFlags.oneLapToGreen),
                randomWaving: sessionFlags.HasFlag(SessionFlags.randomWaving),
                red: sessionFlags.HasFlag(SessionFlags.red),
                repair: sessionFlags.HasFlag(SessionFlags.repair),
                servicible: sessionFlags.HasFlag(SessionFlags.servicible),
                startGo: sessionFlags.HasFlag(SessionFlags.startGo),
                startHidden: sessionFlags.HasFlag(SessionFlags.startHidden),
                startReady: sessionFlags.HasFlag(SessionFlags.startReady),
                startSet: sessionFlags.HasFlag(SessionFlags.startSet),
                tenToGo: sessionFlags.HasFlag(SessionFlags.tenToGo),
                white: sessionFlags.HasFlag(SessionFlags.white),
                yellow: sessionFlags.HasFlag(SessionFlags.yellow),
                yellowWaving: sessionFlags.HasFlag(SessionFlags.yellowWaving)
            );

            if (state.LastRaceFlags?.DifferentTo(@event) != true || state.SendRaceFlags)
            {
                if (@event.Green)
                {
                    // We can trust timings
                    foreach (var info in state.CarsTracked)
                    {
                        // We dont need to observe laps, just start
                        info.Value.ObservedCrossFinishingLine = 3;
                    }
                }

                EventBus.PublishEvent(@event);
                state.LastRaceFlags = @event;
                state.SendRaceFlags = false;
            }
        }
    }
}