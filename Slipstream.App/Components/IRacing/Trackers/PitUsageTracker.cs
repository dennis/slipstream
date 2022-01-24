using Slipstream.Components.IRacing.Models;
using Slipstream.Shared;

using System;

#nullable enable

namespace Slipstream.Components.IRacing.Trackers
{
    internal class PitUsageTracker : IIRacingDataTracker
    {
        private readonly IIRacingEventFactory EventFactory;
        private readonly IEventBus EventBus;

        public PitUsageTracker(IEventBus eventBus, IIRacingEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void Handle(GameState.IState currentState, IRacingDataTrackerState state, IEventEnvelope envelope)
        {
            foreach (var car in currentState.Cars)
            {
                var onPitRoad = car.OnPitRoad;

                if (!state.CarsTracked.TryGetValue(car.CarIdx, out CarState? carState))
                {
                    carState = CarState.Build(car.CarIdx, currentState);
                    state.CarsTracked.Add(car.CarIdx, carState);
                }

                // Ignore car data if the location is NotInWord. In team events when changing drivers, it seems that the
                // car is NotInWorld before returning to pits - and that will give two PitEnter events. To avoid this, we
                // ignore data with NotInWorld.. NotInWorld also happens when you blink, but we can also ignore these.
                if (carState.LastOnPitRoad != onPitRoad && car.Location != IIRacingEventFactory.CarLocation.NotInWorld)
                {
                    var localUser = currentState.DriverCarIdx == car.CarIdx;
                    var now = currentState.SessionTime;
                    if (onPitRoad)
                    {
                        EventBus.PublishEvent(EventFactory.CreateIRacingPitEnter(
                            envelope: envelope,
                            sessionTime: now,
                            carIdx: car.CarIdx,
                            localUser: localUser));
                        carState.PitEnteredAt = currentState.SessionTime;
                    }
                    else
                    {
                        double? duration = null;

                        if (carState.PitEnteredAt != null)
                            duration = currentState.SessionTime - carState.PitEnteredAt;

                        // If duration is zero, then the pitstop started before we joined
                        if (duration != null)
                        {
                            float? fuelLeft = null;

                            if (localUser)
                            {
                                fuelLeft = currentState.FuelLevel;
                            }

                            EventBus.PublishEvent(EventFactory.CreateIRacingPitExit(
                                envelope: envelope,
                                sessionTime: now,
                                carIdx: car.CarIdx,
                                localUser: localUser,
                                duration: duration,
                                fuelLeft: fuelLeft
                            ));
                            carState.PitEnteredAt = null;

                            if (localUser && car.LapsCompleted > 0)
                            {
                                var status = EventFactory.CreateIRacingPitstopReport(
                                    envelope: envelope,
                                    sessionTime: now,
                                    carIdx: car.CarIdx,

                                    tempLFL: (uint)Math.Round(currentState.LFtempCL * 100),
                                    tempLFM: (uint)Math.Round(currentState.LFtempCM * 100),
                                    tempLFR: (uint)Math.Round(currentState.LFtempCR * 100),

                                    tempRFL: (uint)Math.Round(currentState.RFtempCL * 100),
                                    tempRFM: (uint)Math.Round(currentState.RFtempCM * 100),
                                    tempRFR: (uint)Math.Round(currentState.RFtempCR * 100),

                                    tempLRL: (uint)Math.Round(currentState.LRtempCL * 100),
                                    tempLRM: (uint)Math.Round(currentState.LRtempCM * 100),
                                    tempLRR: (uint)Math.Round(currentState.LRtempCR * 100),

                                    tempRRL: (uint)Math.Round(currentState.RRtempCL * 100),
                                    tempRRM: (uint)Math.Round(currentState.RRtempCM * 100),
                                    tempRRR: (uint)Math.Round(currentState.RRtempCR * 100),

                                    wearLFL: (uint)Math.Round(currentState.LFwearL * 100),
                                    wearLFM: (uint)Math.Round(currentState.LFwearM * 100),
                                    wearLFR: (uint)Math.Round(currentState.LFwearR * 100),

                                    wearRFL: (uint)Math.Round(currentState.RFwearL * 100),
                                    wearRFM: (uint)Math.Round(currentState.RFwearM * 100),
                                    wearRFR: (uint)Math.Round(currentState.RFwearR * 100),

                                    wearLRL: (uint)Math.Round(currentState.LRwearL * 100),
                                    wearLRM: (uint)Math.Round(currentState.LRwearM * 100),
                                    wearLRR: (uint)Math.Round(currentState.LRwearR * 100),

                                    wearRRL: (uint)Math.Round(currentState.RRwearL * 100),
                                    wearRRM: (uint)Math.Round(currentState.RRwearM * 100),
                                    wearRRR: (uint)Math.Round(currentState.RRwearR * 100),

                                    laps: car.LapsCompleted - carState.StintStartLap,
                                    fuelDelta: currentState.FuelLevel - carState.StintFuelLevel,
                                    duration: now - carState.StintStartTime
                                );

                                EventBus.PublishEvent(status);
                            }
                        }

                        carState.StintStartLap = car.LapsCompleted;
                        carState.StintFuelLevel = currentState.FuelLevel;
                        carState.StintStartTime = now;
                    }

                    carState.LastOnPitRoad = onPitRoad;
                }
            }
        }

        public void Request(GameState.IState currentState, IRacingDataTrackerState state, IEventEnvelope envelope, IIRacingDataTracker.RequestType request)
        {
        }
    }
}