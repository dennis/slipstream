using iRacingSDK;
using Slipstream.Shared;
using Slipstream.Shared.Events.IRacing;
using Slipstream.Shared.Factories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using static Slipstream.Shared.Factories.IIRacingEventFactory;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    internal class IRacingPlugin : BasePlugin
    {
        private const int MAX_CARS = 64;
        private readonly iRacingConnection Connection = new iRacingConnection();
        private readonly IIRacingEventFactory EventFactory;
        private readonly IEventBus EventBus;

        private class DriverState
        {
            public int PlayerCarDriverIncidentCount { get; set; }

            public void ClearState()
            {
                PlayerCarDriverIncidentCount = 0;
            }
        }

        private class CarState
        {
            public bool LastOnPitRoad { get; set; }
            public int LastLap { get; set; }
            public double LastLapTime { get; set; }
            public float FuelLevelLastLap { get; set; }
            public IRacingCarInfo CarInfo { get; set; } = new IRacingCarInfo();
            public int ObservedCrossFinishingLine { get; set; }
            public double? PitEnteredAt { get; set; }
            public int StintStartLap { get; set; }
            public float StintFuelLevel { get; set; }
            public double StintStartTime { get; set; }

            public void ClearState() // This is invoked at start of session, so we set ObservedCrossFinishingLine higher than initially
            {
                LastOnPitRoad = false;
                LastLap = -1;
                FuelLevelLastLap = -1;
                ObservedCrossFinishingLine = 2; // as we are starting on lap 0, we need a full lap before reporting
                PitEnteredAt = null;
                StintFuelLevel = -1;
                StintStartTime = 0;
            }
        }

        private readonly IDictionary<long, CarState> CarsTracked = new Dictionary<long, CarState>();

        private static readonly Dictionary<iRacingSDK.SessionState, IRacingSessionStateEnum> SessionStateMapping = new Dictionary<iRacingSDK.SessionState, IRacingSessionStateEnum>() {
            { iRacingSDK.SessionState.Checkered, IRacingSessionStateEnum.Checkered },
            { iRacingSDK.SessionState.CoolDown, IRacingSessionStateEnum.CoolDown },
            { iRacingSDK.SessionState.GetInCar, IRacingSessionStateEnum.GetInCar },
            { iRacingSDK.SessionState.Invalid, IRacingSessionStateEnum.Invalid },
            { iRacingSDK.SessionState.ParadeLaps, IRacingSessionStateEnum.ParadeLaps },
            { iRacingSDK.SessionState.Racing, IRacingSessionStateEnum.Racing },
            { iRacingSDK.SessionState.Warmup, IRacingSessionStateEnum.Warmup },
        };

        private static readonly Dictionary<string, IRacingSessionTypeEnum> IRacingSessionTypes = new Dictionary<string, IRacingSessionTypeEnum>()
        {
            { "Practice", IRacingSessionTypeEnum.Practice },
            { "Open Qualify", IRacingSessionTypeEnum.OpenQualify },
            { "Lone Qualify", IRacingSessionTypeEnum.LoneQualify },
            { "Offline Testing", IRacingSessionTypeEnum.OfflineTesting },
            { "Race", IRacingSessionTypeEnum.Race },
            { "Warmup", IRacingSessionTypeEnum.Warmup },
        };

        private static readonly Dictionary<string, IRacingCategoryEnum> IRacingCategoryTypes = new Dictionary<string, IRacingCategoryEnum>()
        {
            { "Road", IRacingCategoryEnum.Road },
            { "Oval", IRacingCategoryEnum.Oval },
            { "DirtRoad", IRacingCategoryEnum.DirtRoad },
            { "DirtOval", IRacingCategoryEnum.DirtOval },
        };

        private readonly int[] LastPositionInClass = new int[MAX_CARS];
        private readonly int[] LastPositionInRace = new int[MAX_CARS];
        private IRacingSessionTypeEnum LastSessionType;
        private SessionState LastSessionState;
        private IRacingRaceFlags? LastRaceFlags;
        private IRacingWeatherInfo? LastWeatherInfo;
        private readonly DriverState driverState = new DriverState();
        private bool Connected;

        private bool SendTrackInfo = false;
        private bool SendCarInfo = false;
        private bool SendWeatherInfo = false;
        private bool SendSessionState = false;
        private bool SendRaceFlags = false;

        public IRacingPlugin(IEventHandlerController eventHandlerController, string id, IIRacingEventFactory eventFactory, IEventBus eventBus) : base(eventHandlerController, id, "IRacingPlugin", id)
        {
            EventFactory = eventFactory;
            EventBus = eventBus;

            var IRacingEventHandler = EventHandlerController.Get<Slipstream.Shared.EventHandlers.IRacing>();

            IRacingEventHandler.OnIRacingCommandSendCarInfo += (s, e) => SendCarInfo = true;
            IRacingEventHandler.OnIRacingCommandSendTrackInfo += (s, e) => SendTrackInfo = true;
            IRacingEventHandler.OnIRacingCommandSendWeatherInfo += (s, e) => SendWeatherInfo = true;
            IRacingEventHandler.OnIRacingCommandSendSessionState += (s, e) => SendSessionState = true;
            IRacingEventHandler.OnIRacingCommandSendRaceFlags += (s, e) => SendRaceFlags = true;
        }

        public override void Loop()
        {
            try
            {
                foreach (var data in Connection.GetDataFeed().WithCorrectedDistances().WithCorrectedPercentages())
                {
                    HandleSample(data);
                    break; // give control back to PluginWorker
                }
            }
            catch (Exception e)
            {
                if (e.Message == "Attempt to read session data before connection to iRacing" || e.Message == "Attempt to read telemetry data before connection to iRacing")
                {
                    if (Connected)
                    {
                        Connected = false;
                        EventBus.PublishEvent(EventFactory.CreateIRacingDisconnected());
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
            HandleCarInfo(data);
            HandleFlags(data);
            HandleLapsCompleted(data);
            HandlePitUsage(data);
            HandleIncident(data);
            HandleIRacingSessionState(data);
            HandlePosition(data);
        }

        private void HandlePosition(DataSample data)
        {
            for (int i = 0; i < data.Telemetry.Cars.Count(); i++)
            {
                int positionInRace = (int)data.Telemetry.CarIdxClassPosition[i];
                int positionInClass = (int)data.Telemetry.CarIdxPosition[i];

                if (positionInRace > 0 && positionInClass > 0)
                {
                    if (positionInRace != LastPositionInRace[i] || positionInClass != LastPositionInClass[i])
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

                LastPositionInClass[i] = positionInClass;
                LastPositionInRace[i] = positionInRace;
            }
        }

        private void HandleIRacingSessionState(DataSample data)
        {
            var sessionData = data.SessionData.SessionInfo.Sessions[data.Telemetry.SessionNum];
            var sessionType = IRacingSessionTypes[sessionData.SessionType];
            var sessionTime = data.Telemetry.SessionTime;

            var category = IRacingCategoryTypes[data.SessionData.WeekendInfo.Category];
            var sessionState = data.Telemetry.SessionState;
            var sessionStateMapped = SessionStateMapping[data.Telemetry.SessionState];
            var lapsLimited = sessionData.IsLimitedSessionLaps;
            var timeLimited = sessionData.IsLimitedTime;
            var totalSessionTime = sessionData._SessionTime / 10_000;
            var totalSessionLaps = sessionData._SessionLaps;

            if (sessionType != LastSessionType || sessionState != LastSessionState || SendSessionState)
            {
                IIRacingSessionState @event =
                   sessionType switch
                   {
                       IRacingSessionTypeEnum.Practice => EventFactory.CreateIRacingPractice(sessionTime: sessionTime, lapsLimited: lapsLimited, timeLimited: timeLimited, totalSessionTime: totalSessionTime, totalSessionLaps: totalSessionLaps, state: sessionStateMapped, category: category),
                       IRacingSessionTypeEnum.OpenQualify => EventFactory.CreateIRacingQualify(sessionTime: sessionTime, lapsLimited: lapsLimited, timeLimited: timeLimited, totalSessionTime: totalSessionTime, totalSessionLaps: totalSessionLaps, state: sessionStateMapped, category: category, openQualify: true),
                       IRacingSessionTypeEnum.LoneQualify => EventFactory.CreateIRacingQualify(sessionTime: sessionTime, lapsLimited: lapsLimited, timeLimited: timeLimited, totalSessionTime: totalSessionTime, totalSessionLaps: totalSessionLaps, state: sessionStateMapped, category: category, openQualify: false),
                       IRacingSessionTypeEnum.OfflineTesting => EventFactory.CreateIRacingTesting(sessionTime: sessionTime, lapsLimited: lapsLimited, timeLimited: timeLimited, totalSessionTime: totalSessionTime, totalSessionLaps: totalSessionLaps, state: sessionStateMapped, category: category),
                       IRacingSessionTypeEnum.Race => EventFactory.CreateIRacingRace(sessionTime: sessionTime, lapsLimited: lapsLimited, timeLimited: timeLimited, totalSessionTime: totalSessionTime, totalSessionLaps: totalSessionLaps, state: sessionStateMapped, category: category),
                       IRacingSessionTypeEnum.Warmup => EventFactory.CreateIRacingWarmup(sessionTime: sessionTime, lapsLimited: lapsLimited, timeLimited: timeLimited, totalSessionTime: totalSessionTime, totalSessionLaps: totalSessionLaps, state: sessionStateMapped, category: category),
                       _ => throw new NotImplementedException(),
                   };

                EventBus.PublishEvent(@event);

                // If we change from practice to qualify, we need to reset car info
                if (sessionType != LastSessionType)
                {
                    foreach (var carState in CarsTracked)
                    {
                        carState.Value.ClearState();
                    }
                }

                for (int i = 0; i < MAX_CARS; i++)
                {
                    LastPositionInClass[i] = 0;
                    LastPositionInRace[i] = 0;
                }

                LastSessionType = sessionType;
                LastSessionState = sessionState;
                SendSessionState = false;
            }
        }

        private void HandleIncident(DataSample data)
        {
            int incidents = Convert.ToInt32(data.Telemetry["PlayerCarDriverIncidentCount"]);
            var incidentDelta = incidents - this.driverState.PlayerCarDriverIncidentCount;

            if (incidentDelta > 0)
            {
                this.driverState.PlayerCarDriverIncidentCount = incidents;
                EventBus.PublishEvent(EventFactory.CreateIRacingDriverIncident(totalIncidents: incidents, incidentDelta: incidentDelta));
            }
        }

        private CarState GetCarState(long idx, DataSample data)
        {
            if (!CarsTracked.TryGetValue(idx, out CarState val))
            {
                val = new CarState
                {
                    StintStartLap = data.Telemetry.CarIdxLapCompleted[idx],
                    StintFuelLevel = data.Telemetry.FuelLevel,
                    StintStartTime = data.Telemetry.SessionTime
                };
                CarsTracked.Add(idx, val);
            }

            return val;
        }

        private void HandlePitUsage(DataSample data)
        {
            for (int i = 0; i < data.Telemetry.CarIdxOnPitRoad.Length; i++)
            {
                var onPitRoad = data.Telemetry.CarIdxOnPitRoad[i];

                var carState = GetCarState(i, data);

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
                var carState = GetCarState(i, data);
                var lapsCompleted = data.Telemetry.CarIdxLapCompleted[i];

                if (lapsCompleted != -1 && carState.LastLap != lapsCompleted)
                {
                    carState.ObservedCrossFinishingLine++;
                    if (lapsCompleted == 0) // This is initial lap, so we can use times from next lap
                        carState.ObservedCrossFinishingLine++;

                    bool localUser = i == data.SessionData.DriverInfo.DriverCarIdx;

                    // 1st time is when leaving the pits / or whatever lap it is on when we join
                    // 2nd time is start of first real lap (that we see in full)
                    // 3rd+ is lap times (we can begin timing laps)
                    // if "we" are doing laps, then we know we joined with the car,
                    // so we can track laps from the 1st one.
                    // for everybody else, we need to see them crossing the line 3 or more times as describe above
                    if (carState.ObservedCrossFinishingLine >= 3 || (localUser && lapsCompleted > 0))
                    {
                        var lapTime = now - carState.LastLapTime;

                        var fuelLeft = data.Telemetry.FuelLevel;

                        float? usedFuel = fuelLeft - carState.FuelLevelLastLap;
                        carState.FuelLevelLastLap = fuelLeft;

                        var @event = EventFactory.CreateIRacingCarCompletedLap(
                            sessionTime: now,
                            carIdx: i,
                            time: lapTime,
                            lapsCompleted: lapsCompleted,
                            fuelDiff: localUser ? usedFuel : null,
                            localUser: localUser);

                        EventBus.PublishEvent(@event);
                    }
                    carState.LastLapTime = now;
                    carState.LastLap = lapsCompleted;
                }
            }
        }

        private void HandleFlags(DataSample data)
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

            if (LastRaceFlags?.DifferentTo(@event) != true || SendRaceFlags)
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
                LastRaceFlags = @event;
                SendRaceFlags = false;
            }
        }

        private void HandleCarInfo(DataSample data)
        {
            foreach (var driver in data.SessionData.DriverInfo.Drivers)
            {
                //if (driver.IsPaceCar) - this doesn't work when doing test sessions. Here we will be flagged as Pace car
                //    continue;

                var carState = GetCarState(driver.CarIdx, data);

                var @event = EventFactory.CreateIRacingCarInfo(
                    sessionTime: data.Telemetry.SessionTime,
                    carIdx: driver.CarIdx,
                    carNumber: driver.CarNumber,
                    currentDriverUserID: driver.UserID,
                    currentDriverName: driver.UserName,
                    teamID: driver.TeamID,
                    teamName: driver.TeamName,
                    carName: driver.CarScreenName,
                    carNameShort: driver.CarScreenNameShort,
                    currentDriverIRating: driver.IRating,
                    currentDriverLicense: driver.LicString,
                    localUser: data.SessionData.DriverInfo.DriverCarIdx == driver.CarIdx,
                    spectator: driver.IsSpectator != 0
                // Seems not to be exposed by SDK:
                // DriverIncidentCount
                // TeamIncidentCount
                );

                if (!carState.CarInfo.SameAs(@event) || SendCarInfo)
                {
                    EventBus.PublishEvent(@event);

                    carState.CarInfo = @event;
                }
            }

            SendCarInfo = false;
        }

        private void HandleWeatherInfo(DataSample data)
        {
            var weatherInfo = EventFactory.CreateIRacingWeatherInfo
            (
                sessionTime: data.Telemetry.SessionTime,
                skies: data.SessionData.WeekendInfo.TrackSkies,
                surfaceTemp: data.SessionData.WeekendInfo.TrackSurfaceTemp,
                airTemp: data.SessionData.WeekendInfo.TrackAirTemp,
                airPressure: data.SessionData.WeekendInfo.TrackAirPressure,
                relativeHumidity: data.SessionData.WeekendInfo.TrackRelativeHumidity,
                fogLevel: data.SessionData.WeekendInfo.TrackFogLevel
            );

            if (LastWeatherInfo == null || !weatherInfo.DifferentTo(LastWeatherInfo) || SendWeatherInfo)
            {
                EventBus.PublishEvent(weatherInfo);

                LastWeatherInfo = weatherInfo;
                SendWeatherInfo = false;
            }
        }

        private void HandleConnect(DataSample data)
        {
            if (!Connected)
            {
                Reset();

                EventBus.PublishEvent(EventFactory.CreateIRacingConnected());

                Connected = true;
                SendTrackInfo = true;
            }

            if (SendTrackInfo)
            {
                EventBus.PublishEvent(EventFactory.CreateIRacingTrackInfo
                (
                    trackId: data.SessionData.WeekendInfo.TrackID,
                    trackLength: data.SessionData.WeekendInfo.TrackLength,
                    trackDisplayName: data.SessionData.WeekendInfo.TrackDisplayName,
                    trackCity: data.SessionData.WeekendInfo.TrackCity,
                    trackCountry: data.SessionData.WeekendInfo.TrackCountry,
                    trackDisplayShortName: data.SessionData.WeekendInfo.TrackDisplayShortName,
                    trackConfigName: data.SessionData.WeekendInfo.TrackConfigName,
                    trackType: data.SessionData.WeekendInfo.TrackType
                ));

                SendTrackInfo = false;
            }
        }

        private void Reset()
        {
            CarsTracked.Clear();
            driverState.ClearState();
            LastRaceFlags = null;
            LastWeatherInfo = null;
            LastSessionState = SessionState.Invalid;
            Connected = false;
        }
    }
}