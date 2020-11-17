using iRacingSDK;
using Slipstream.Shared;
using Slipstream.Shared.Events.IRacing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    static class Helpers
    {
        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key) where TValue : new()
        {
            if (!dict.TryGetValue(key, out TValue val))
            {
                val = new TValue();
                dict.Add(key, val);
            }

            return val;
        }
    }

    class IRacingPlugin : IPlugin
    {
        public string Id { get; }
        public string Name => "IRacingPlugin";
        public string DisplayName => Name;
        public bool Enabled { get; set; }
        public string WorkerName => Name;

        private readonly iRacingConnection Connection = new iRacingConnection();
        private readonly Shared.IEventBus EventBus;
        private readonly Dictionary<string, IRacingCurrentSession.SessionTypes> SessionTypeMapping = new Dictionary<string, IRacingCurrentSession.SessionTypes>() {
            { "Practice", IRacingCurrentSession.SessionTypes.Practice },
            { "Open Qualify", IRacingCurrentSession.SessionTypes.OpenQualify },
            { "Lone Qualify", IRacingCurrentSession.SessionTypes.LoneQualify },
            { "Offline Testing", IRacingCurrentSession.SessionTypes.OfflineTesting },
            { "Race", IRacingCurrentSession.SessionTypes.Race },
            { "Warmup", IRacingCurrentSession.SessionTypes.Warmup }
        };

        private IEventBusSubscription? Subscription;
        private readonly Shared.EventHandler PreEventHandler = new Shared.EventHandler();
        private bool SeenPluginsReady;

        private class CarState
        {
            public bool LastOnPitRoad { get; set; }
            public int LastLap { get; set; }
            public double LastLapTime { get; set; }
            public float FuelLevelLastLap { get; set; }
            public IRacingCarInfo CarInfo { get; set; } = new IRacingCarInfo();
            public int ObservedCrossFinishingLine { get; set; }
            public double? PitEnteredAt { get; set; }
        }

        private readonly IDictionary<long, CarState> CarsTracked = new Dictionary<long, CarState>();

        private readonly Dictionary<iRacingSDK.SessionState, IRacingSessionState.StateEnum> SessionStateMapping = new Dictionary<iRacingSDK.SessionState, IRacingSessionState.StateEnum>() {
            { iRacingSDK.SessionState.Checkered, IRacingSessionState.StateEnum.Checkered },
            { iRacingSDK.SessionState.CoolDown, IRacingSessionState.StateEnum.CoolDown },
            { iRacingSDK.SessionState.GetInCar, IRacingSessionState.StateEnum.GetInCar },
            { iRacingSDK.SessionState.Invalid, IRacingSessionState.StateEnum.Invalid },
            { iRacingSDK.SessionState.ParadeLaps, IRacingSessionState.StateEnum.ParadeLaps },
            { iRacingSDK.SessionState.Racing, IRacingSessionState.StateEnum.Racing },
            { iRacingSDK.SessionState.Warmup, IRacingSessionState.StateEnum.Warmup },
        };

        private IRacingCurrentSession? lastSessionInfo;
        private IRacingSessionState? LastSessionState;
        private IRacingRaceFlags? lastRaceFlags;
        private IRacingWeatherInfo? lastWeatherInfo;
        private bool connected;

        public IRacingPlugin(string id, IEventBus eventBus)
        {
            Id = id;
            EventBus = eventBus;
            Subscription = EventBus.RegisterListener();

            PreEventHandler.OnInternalPluginsReady += (s, e) =>
            {
                Subscription.Dispose();
                Subscription = null;
                SeenPluginsReady = true;

                Thread.Sleep(1000); // Let other plugins be ready
            };
        }

        public void Disable(IEngine engine)
        {
            Enabled = false;
        }

        public void Enable(IEngine engine)
        {
            Enabled = true;
            Reset();
        }

        public void RegisterPlugin(IEngine engine)
        {
        }

        public void UnregisterPlugin(IEngine engine)
        {
        }

        public void Loop()
        {
            // Dont send any events until all plugins are ready
            if (!SeenPluginsReady)
            {
                PreEventHandler.HandleEvent(Subscription?.NextEvent(10));
                return;
            }

            try
            {
                foreach (var data in Connection.GetDataFeed().WithCorrectedDistances().WithCorrectedPercentages())
                {
                    HandleSample(data);
                    break; // give control back to Worker
                }
            }
            catch (Exception e)
            {
                if (e.Message == "Attempt to read session data before connection to iRacing" || e.Message == "Attempt to read telemetry data before connection to iRacing")
                {
                    if (connected)
                    {
                        connected = false;
                        EventBus.PublishEvent(new IRacingDisconnected());
                    }
                    Thread.Sleep(5000);
                }

                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
            }
        }

        private void HandleSample(DataSample data)
        {
            HandleConnect(data);
            HandleWeatherInfo(data);
            HandleCurrentSession(data);
            HandleCarInfo(data);
            HandleFlags(data);
            HandleState(data);
            HandleLapsCompleted(data);
            HandlePitUsage(data);
        }

        private void HandlePitUsage(DataSample data)
        {
            for (int i = 0; i < data.Telemetry.CarIdxOnPitRoad.Length; i++)
            {
                var onPitRoad = data.Telemetry.CarIdxOnPitRoad[i];

                var carState = CarsTracked.GetOrCreate(i);

                if (carState.LastOnPitRoad != onPitRoad)
                {
                    var localUser = data.SessionData.DriverInfo.DriverCarIdx == i;
                    var now = data.Telemetry.SessionTime;
                    if (onPitRoad)
                    {
                        EventBus.PublishEvent(new IRacingPitEnter { CarIdx = i, LocalUser = localUser, SessionTime = now });
                        carState.PitEnteredAt = data.Telemetry.SessionTime;

                        if (localUser)
                        {
                            var status = new IRacingCarStatus
                            {
                                SessionTime = now,
                                CarIdx = i,

                                TempLFL = data.Telemetry.LFtempCL, TempLFM = data.Telemetry.LFtempCM, TempLFR = data.Telemetry.LFtempCR,
                                TempRFL = data.Telemetry.RFtempCL, TempRFM = data.Telemetry.RFtempCM, TempRFR = data.Telemetry.RFtempCR,
                                TempLRL = data.Telemetry.RFtempCL, TempLRM = data.Telemetry.LRtempCM, TempLRR = data.Telemetry.LRtempCR,
                                TempRRL = data.Telemetry.RFtempCL, TempRRM = data.Telemetry.RRtempCM, TempRRR = data.Telemetry.RRtempCR,

                                WearLFL = data.Telemetry.LFwearL, WearLFM = data.Telemetry.LFwearM, WearLFR = data.Telemetry.LFwearR,
                                WearRFL = data.Telemetry.RFwearL, WearRFM = data.Telemetry.RFwearM, WearRFR = data.Telemetry.RFwearR,
                                WearLRL = data.Telemetry.LRwearL, WearLRM = data.Telemetry.LRwearM, WearLRR = data.Telemetry.LRwearR,
                                WearRRL = data.Telemetry.RRwearL, WearRRM = data.Telemetry.RRwearM, WearRRR = data.Telemetry.RRwearR,
                            };

                            EventBus.PublishEvent(status);
                        }
                    }
                    else
                    {
                        double? duration = null;

                        if (carState.PitEnteredAt != null)
                            duration = data.Telemetry.SessionTime - carState.PitEnteredAt;

                        EventBus.PublishEvent(new IRacingPitExit { CarIdx = i, LocalUser = localUser, SessionTime = now, Duration = duration });
                        carState.PitEnteredAt = null;
                    }

                    carState.LastOnPitRoad = onPitRoad;
                }
            }
        }

        private void HandleLapsCompleted(DataSample data)
        {
            // We cant use data.Telemetry.Cars[].LastTime, as it wont contain data if 
            // the driver had an incident. So we calculate it ourself which will be slightly
            // off compared to IRacings own timing.
            // If you plan to use LastTime, please remember to wait approx 2.5 before reading 
            // LastTime, as the value is updated with about 2.5s delay
            var now = data.Telemetry.SessionTime;

            for (int i = 0; i < data.Telemetry.Cars.Count(); i++)
            {
                var carState = CarsTracked.GetOrCreate(i);
                var lapsCompleted = data.Telemetry.CarIdxLapCompleted[i];

                if (i == data.SessionData.DriverInfo.DriverCarIdx)
                {
                    //Debug.WriteLine($"FuelLevel={data.Telemetry.FuelLevel}, FuelLevelPct={data.Telemetry.FuelLevelPct}, FuelPress={data.Telemetry.FuelPress}, FuelUsePerHour={data.Telemetry.FuelUsePerHour}");
                }

                if (lapsCompleted != -1 && carState.LastLap != lapsCompleted)
                {
                    carState.ObservedCrossFinishingLine += 1;
                    Debug.WriteLine($"car-{i} passing finishing line at {now} at count {carState.ObservedCrossFinishingLine}");

                    // 1st time is when leaving the pits / or whatever lap it is on when we join
                    // 2nd time is start of first real lap (that we see in full)
                    // 3rd+ is lap times (we can begin timing laps)
                    if (carState.ObservedCrossFinishingLine >= 3)
                    {
                        var lapTime = now - carState.LastLapTime;

                        bool localUser = i == data.SessionData.DriverInfo.DriverCarIdx;

                        var fuelLeft = data.Telemetry.FuelLevel;

                        float? usedFuel = fuelLeft - carState.FuelLevelLastLap;
                        carState.FuelLevelLastLap = fuelLeft;

                        Debug.WriteLine($"car-{i} now={now}, LastLapTime={carState.LastLapTime}, LastLap={carState.LastLap}, lapsCompleted={lapsCompleted}, fuelLeft={fuelLeft}, usedFuel={usedFuel}");
                        Debug.WriteLine($"car-{i} lap {lapsCompleted} time {lapTime} fuel {usedFuel}");

                        var @event = new IRacingCarCompletedLap
                        {
                            SessionTime = now,
                            CarIdx = i,
                            Time = lapTime,
                            LapsComplete = lapsCompleted,
                            LocalUser = localUser,
                            FuelDiff = localUser ? usedFuel : null,
                        };

                        EventBus.PublishEvent(@event);
                    }
                    carState.LastLapTime = now;
                    carState.LastLap = lapsCompleted;
                }
            }
        }

        private void HandleState(DataSample data)
        {
            var @event = new IRacingSessionState
            {
                SessionTime = data.Telemetry.SessionTime,
                State = SessionStateMapping[data.Telemetry.SessionState]
            };

            if(LastSessionState == null || !LastSessionState.DifferentTo(@event))
            {
                EventBus.PublishEvent(@event);
                LastSessionState = @event;
            }
        }

        private void HandleFlags(DataSample data)
        {
            var sessionFlags = data.Telemetry.SessionFlags;

            var @event = new IRacingRaceFlags 
            {
                SessionTime = data.Telemetry.SessionTime,
                Black = sessionFlags.HasFlag(SessionFlags.black),
                Blue = sessionFlags.HasFlag(SessionFlags.blue),
                Caution = sessionFlags.HasFlag(SessionFlags.caution),
                CautionWaving = sessionFlags.HasFlag(SessionFlags.cautionWaving),
                Checkered = sessionFlags.HasFlag(SessionFlags.checkered),
                Crossed = sessionFlags.HasFlag(SessionFlags.crossed),
                Debris = sessionFlags.HasFlag(SessionFlags.debris),
                Disqualify = sessionFlags.HasFlag(SessionFlags.disqualify),
                FiveToGo = sessionFlags.HasFlag(SessionFlags.fiveToGo),
                Furled = sessionFlags.HasFlag(SessionFlags.furled),
                Green = sessionFlags.HasFlag(SessionFlags.green),
                GreenHeld = sessionFlags.HasFlag(SessionFlags.greenHeld),
                OneLapToGreen = sessionFlags.HasFlag(SessionFlags.oneLapToGreen),
                RandomWaving = sessionFlags.HasFlag(SessionFlags.randomWaving),
                Red = sessionFlags.HasFlag(SessionFlags.red),
                Repair = sessionFlags.HasFlag(SessionFlags.repair),
                Servicible = sessionFlags.HasFlag(SessionFlags.servicible),
                StartGo = sessionFlags.HasFlag(SessionFlags.startGo),
                StartHidden = sessionFlags.HasFlag(SessionFlags.startHidden),
                StartReady = sessionFlags.HasFlag(SessionFlags.startReady),
                StartSet = sessionFlags.HasFlag(SessionFlags.startSet),
                TenToGo = sessionFlags.HasFlag(SessionFlags.tenToGo),
                White = sessionFlags.HasFlag(SessionFlags.white),
                Yellow = sessionFlags.HasFlag(SessionFlags.yellow),
                YellowWaving = sessionFlags.HasFlag(SessionFlags.yellowWaving),
            };

            if(lastRaceFlags == null || !lastRaceFlags.DifferentTo(@event))
            {
                if (@event.Green)
                {
                    // We can trust timings
                    foreach (var info in CarsTracked)
                    {
                        // We dont need to observe laps, just start
                        info.Value.ObservedCrossFinishingLine = 3;
                    }
                }

                EventBus.PublishEvent(@event);
                lastRaceFlags = @event;
            }
        }

        private void HandleCarInfo(DataSample data)
        {
            foreach (var driver in data.SessionData.DriverInfo.Drivers)
            {
                if (driver.IsPaceCar)
                    continue;

                var carState = CarsTracked.GetOrCreate(driver.CarIdx);

                var @event = new IRacingCarInfo
                {
                    SessionTime = data.Telemetry.SessionTime,
                    CarIdx = driver.CarIdx,
                    CarNumber = driver.CarNumber,
                    CurrentDriverUserID = driver.UserID,
                    CurrentDriverName = driver.UserName,
                    TeamID = driver.TeamID,
                    TeamName = driver.TeamName,
                    CarName = driver.CarScreenName,
                    CarNameShort = driver.CarScreenNameShort,
                    CurrentDriverIRating = driver.IRating,
                    LocalUser = data.SessionData.DriverInfo.DriverCarIdx == driver.CarID,
                    Spectator = driver.IsSpectator,
                    // Seems not to be exposed by SDK:
                    // DriverIncidentCount
                    // TeamIncidentCount
                };

                if (!carState.CarInfo.SameAs(@event))
                {
                    EventBus.PublishEvent(@event);

                    carState.CarInfo = @event;
                    Debug.WriteLine($"SEnding CArInfo {@event}");
                }
            }
        }

        private void HandleCurrentSession(DataSample data)
        {
            var sessionData = data.SessionData.SessionInfo.Sessions[data.Telemetry.SessionNum];
            var sessionInfo = new IRacingCurrentSession
            {
                SessionType = SessionTypeMapping[sessionData.SessionType],
                TimeLimited = sessionData.IsLimitedTime,
                LapsLimited = sessionData.IsLimitedSessionLaps,
                TotalSessionLaps = sessionData._SessionLaps,
                TotalSessionTime = sessionData._SessionTime / 10_000,
            };

            if (lastSessionInfo == null || !sessionInfo.Equals(lastSessionInfo))
            {
                EventBus.PublishEvent(sessionInfo);

                lastSessionInfo = sessionInfo;
            }
        }

        private void HandleWeatherInfo(DataSample data)
        {
            var weatherInfo = new IRacingWeatherInfo
            {
                SessionTime = data.Telemetry.SessionTime,
                Skies = data.SessionData.WeekendInfo.TrackSkies,
                SurfaceTemp = data.SessionData.WeekendInfo.TrackSurfaceTemp,
                AirTemp = data.SessionData.WeekendInfo.TrackAirTemp,
                AirPressure = data.SessionData.WeekendInfo.TrackAirPressure,
                RelativeHumidity = data.SessionData.WeekendInfo.TrackRelativeHumidity,
                FogLevel = data.SessionData.WeekendInfo.TrackFogLevel,
            };

            if (lastWeatherInfo == null || !weatherInfo.DifferentTo(lastWeatherInfo))
            {
                EventBus.PublishEvent(weatherInfo);

                lastWeatherInfo = weatherInfo;
            }
        }

        private void HandleConnect(DataSample data)
        {
            if (!connected)
            {
                lastWeatherInfo = null;
                lastSessionInfo = null;
                connected = true;

                EventBus.PublishEvent(new IRacingConnected());
                EventBus.PublishEvent(new IRacingTrackInfo
                {
                    TrackId = data.SessionData.WeekendInfo.TrackID,
                    TrackLength = data.SessionData.WeekendInfo.TrackLength,
                    TrackDisplayName = data.SessionData.WeekendInfo.TrackDisplayName,
                    TrackCity = data.SessionData.WeekendInfo.TrackCity,
                    TrackCountry = data.SessionData.WeekendInfo.TrackCountry,
                    TrackDisplayShortName = data.SessionData.WeekendInfo.TrackDisplayShortName,
                    TrackConfigName = data.SessionData.WeekendInfo.TrackConfigName,
                    TrackType = data.SessionData.WeekendInfo.TrackType,
                });
            }
        }

        private void Reset()
        {
            CarsTracked.Clear();
            lastSessionInfo = null;
            LastSessionState = null;
            lastRaceFlags = null;
            lastWeatherInfo = null;
            connected = false;
        }
    }
}
