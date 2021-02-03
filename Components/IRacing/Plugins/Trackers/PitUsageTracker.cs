using iRacingSDK;
using Slipstream.Components.IRacing.Plugins.Models;
using Slipstream.Shared;
using System;

#nullable enable

namespace Slipstream.Components.IRacing.Plugins.Trackers
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

        public void Handle(DataSample data, IRacingDataTrackerState state)
        {
            for (int i = 0; i < data.Telemetry.CarIdxOnPitRoad.Length; i++)
            {
                var onPitRoad = data.Telemetry.CarIdxOnPitRoad[i];

                if (!state.CarsTracked.TryGetValue(i, out CarState carState))
                {
                    carState = CarState.Build(i, data);
                    state.CarsTracked.Add(i, carState);
                }

                if (carState.LastOnPitRoad != onPitRoad)
                {
                    var localUser = data.SessionData.DriverInfo.DriverCarIdx == i;
                    var now = data.Telemetry.SessionTime;
                    if (onPitRoad)
                    {
                        EventBus.PublishEvent(EventFactory.CreateIRacingPitEnter(sessionTime: now, carIdx: i, localUser: localUser));
                        carState.PitEnteredAt = data.Telemetry.SessionTime;
                    }
                    else
                    {
                        double? duration = null;

                        if (carState.PitEnteredAt != null)
                            duration = data.Telemetry.SessionTime - carState.PitEnteredAt;

                        // If duration is zero, then the pitstop started before we joined
                        if (duration != null)
                        {
                            EventBus.PublishEvent(EventFactory.CreateIRacingPitExit(sessionTime: now, carIdx: i, localUser: localUser, duration: duration));
                            carState.PitEnteredAt = null;

                            if (localUser && data.Telemetry.CarIdxLapCompleted[i] > 0)
                            {
                                var status = EventFactory.CreateIRacingPitstopReport(
                                    sessionTime: now,
                                    carIdx: i,

                                    tempLFL: (uint)Math.Round(data.Telemetry.LFtempCL),
                                    tempLFM: (uint)Math.Round(data.Telemetry.LFtempCM),
                                    tempLFR: (uint)Math.Round(data.Telemetry.LFtempCR),

                                    tempRFL: (uint)Math.Round(data.Telemetry.RFtempCL),
                                    tempRFM: (uint)Math.Round(data.Telemetry.RFtempCM),
                                    tempRFR: (uint)Math.Round(data.Telemetry.RFtempCR),

                                    tempLRL: (uint)Math.Round(data.Telemetry.LRtempCL),
                                    tempLRM: (uint)Math.Round(data.Telemetry.LRtempCM),
                                    tempLRR: (uint)Math.Round(data.Telemetry.LRtempCR),

                                    tempRRL: (uint)Math.Round(data.Telemetry.RRtempCL),
                                    tempRRM: (uint)Math.Round(data.Telemetry.RRtempCM),
                                    tempRRR: (uint)Math.Round(data.Telemetry.RRtempCR),

                                    wearLFL: (uint)Math.Round(data.Telemetry.LFwearL * 100),
                                    wearLFM: (uint)Math.Round(data.Telemetry.LFwearM * 100),
                                    wearLFR: (uint)Math.Round(data.Telemetry.LFwearR * 100),

                                    wearRFL: (uint)Math.Round(data.Telemetry.RFwearL * 100),
                                    wearRFM: (uint)Math.Round(data.Telemetry.RFwearM * 100),
                                    wearRFR: (uint)Math.Round(data.Telemetry.RFwearR * 100),

                                    wearLRL: (uint)Math.Round(data.Telemetry.LRwearL * 100),
                                    wearLRM: (uint)Math.Round(data.Telemetry.LRwearM * 100),
                                    wearLRR: (uint)Math.Round(data.Telemetry.LRwearR * 100),

                                    wearRRL: (uint)Math.Round(data.Telemetry.RRwearL * 100),
                                    wearRRM: (uint)Math.Round(data.Telemetry.RRwearM * 100),
                                    wearRRR: (uint)Math.Round(data.Telemetry.RRwearR * 100),

                                    laps: data.Telemetry.CarIdxLapCompleted[i] - carState.StintStartLap,
                                    fuelDiff: data.Telemetry.FuelLevel - carState.StintFuelLevel,
                                    duration: now - carState.StintStartTime
                                );

                                EventBus.PublishEvent(status);
                            }
                        }

                        carState.StintStartLap = data.Telemetry.CarIdxLapCompleted[i];
                        carState.StintFuelLevel = data.Telemetry.FuelLevel;
                        carState.StintStartTime = now;
                    }

                    carState.LastOnPitRoad = onPitRoad;
                }
            }
        }
    }
}