#nullable enable

using iRacingSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using static Slipstream.Components.IRacing.IIRacingEventFactory;

namespace Slipstream.Components.IRacing.Plugins.GameState
{
    internal class StateFactory : IStateFactory
    {
        private readonly Dictionary<int, Car> Cars = new Dictionary<int, Car>();

        private static readonly Dictionary<iRacingSDK.SessionState, IRacingSessionStateEnum> SessionStateMapping = new Dictionary<iRacingSDK.SessionState, IRacingSessionStateEnum>() {
            { iRacingSDK.SessionState.Checkered, IRacingSessionStateEnum.Checkered },
            { iRacingSDK.SessionState.CoolDown, IRacingSessionStateEnum.CoolDown },
            { iRacingSDK.SessionState.GetInCar, IRacingSessionStateEnum.GetInCar },
            { iRacingSDK.SessionState.Invalid, IRacingSessionStateEnum.Invalid },
            { iRacingSDK.SessionState.ParadeLaps, IRacingSessionStateEnum.ParadeLaps },
            { iRacingSDK.SessionState.Racing, IRacingSessionStateEnum.Racing },
            { iRacingSDK.SessionState.Warmup, IRacingSessionStateEnum.Warmup },
        };

        private static readonly Dictionary<string, IRacingCategoryEnum> IRacingCategoryTypes = new Dictionary<string, IRacingCategoryEnum>()
        {
            { "Road", IRacingCategoryEnum.Road },
            { "Oval", IRacingCategoryEnum.Oval },
            { "DirtRoad", IRacingCategoryEnum.DirtRoad },
            { "DirtOval", IRacingCategoryEnum.DirtOval },
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

        public IState? BuildState(DataSample ds)
        {
            IRacingSessionTypeEnum sessionType = IRacingSessionTypeEnum.Practice; // Default

            // IRacing sometimes gives SessionNum == -1 back, so we need handle that
            if (ds.Telemetry.SessionNum >= 0)
            {
                sessionType = IRacingSessionTypes[ds.SessionData.SessionInfo.Sessions[ds.Telemetry.SessionNum].SessionType];
            }

            var state = new State
            {
                SessionTime = ds.Telemetry.SessionTime,
                SessionNum = Math.Max(0, ds.Telemetry.SessionNum),
                SessionFlags = (SessionFlags)(int)ds.Telemetry.SessionFlags,
                SessionState = SessionStateMapping[ds.Telemetry.SessionState],
                SessionType = sessionType,

                DriverCarIdx = ds.SessionData.DriverInfo.DriverCarIdx,
                DriverIncidentCount = Convert.ToInt32(ds.Telemetry["PlayerCarDriverIncidentCount"]),
                MyIncidentCount = Convert.ToInt32(ds.Telemetry["PlayerCarMyIncidentCount"]),
                TeamIncidentCount = Convert.ToInt32(ds.Telemetry["PlayerCarTeamIncidentCount"]),
                FuelLevel = ds.Telemetry.FuelLevel,

                LFtempCL = ds.Telemetry.LFtempCL,
                LFtempCM = ds.Telemetry.LFtempCM,
                LFtempCR = ds.Telemetry.LFtempCR,

                RFtempCL = ds.Telemetry.RFtempCL,
                RFtempCM = ds.Telemetry.RFtempCM,
                RFtempCR = ds.Telemetry.RFtempCR,

                LRtempCL = ds.Telemetry.LRtempCL,
                LRtempCM = ds.Telemetry.LRtempCM,
                LRtempCR = ds.Telemetry.LRtempCR,

                RRtempCL = ds.Telemetry.RRtempCL,
                RRtempCM = ds.Telemetry.RRtempCM,
                RRtempCR = ds.Telemetry.RRtempCR,

                LFwearL = ds.Telemetry.LFwearL,
                LFwearM = ds.Telemetry.LFwearM,
                LFwearR = ds.Telemetry.LFwearR,

                RFwearL = ds.Telemetry.RFwearL,
                RFwearM = ds.Telemetry.RFwearM,
                RFwearR = ds.Telemetry.RFwearR,

                LRwearL = ds.Telemetry.LRwearL,
                LRwearM = ds.Telemetry.LRwearM,
                LRwearR = ds.Telemetry.LRwearR,

                RRwearL = ds.Telemetry.RRwearL,
                RRwearM = ds.Telemetry.RRwearM,
                RRwearR = ds.Telemetry.RRwearR,

                TrackId = ds.SessionData.WeekendInfo.TrackID,
                TrackLength = ds.SessionData.WeekendInfo.TrackLength,
                TrackDisplayName = ds.SessionData.WeekendInfo.TrackDisplayName,
                TrackCity = ds.SessionData.WeekendInfo.TrackCity,
                TrackCountry = ds.SessionData.WeekendInfo.TrackCountry,
                TrackDisplayShortName = ds.SessionData.WeekendInfo.TrackDisplayShortName,
                TrackConfigName = ds.SessionData.WeekendInfo.TrackConfigName,
                TrackType = ds.SessionData.WeekendInfo.TrackType,

                Skies = (IIRacingEventFactory.Skies)(int)ds.Telemetry.Skies,
                TrackTempCrew = ds.Telemetry.TrackTempCrew,
                AirTemp = ds.Telemetry.AirTemp,
                AirPressure = ds.Telemetry.AirPressure,
                RelativeHumidity = ds.Telemetry.RelativeHumidity,
                FogLevel = ds.Telemetry.FogLevel,

                RaceCategory = IRacingCategoryTypes[ds.SessionData.WeekendInfo.Category],

                PlayerCarTowTime = (float)ds.Telemetry["PlayerCarTowTime"],
            };

            {
                List<Car> cars = new List<Car>();
                foreach (var d in ds.SessionData.DriverInfo.Drivers)
                {
                    int idx = (int)d.CarIdx;
                    var location = (IIRacingEventFactory.CarLocation)(int)ds.Telemetry.CarIdxTrackSurface[idx];
                    var lastLapTime = ((float[])ds.Telemetry["CarIdxLastLapTime"])[idx];
                    var bestLapTime = ((float[])ds.Telemetry["CarIdxBestLapTime"])[idx];
                    var bestLapNum = ((int[])ds.Telemetry["CarIdxBestLapNum"])[idx];
                    var lapDistPct = ((float[])ds.Telemetry["CarIdxLapDistPct"])[idx];

                    var car = new Car
                    {
                        CarIdx = idx,
                        CarNumber = d.CarNumber,
                        UserId = d.UserID,
                        UserName = d.UserName,
                        TeamId = d.TeamID,
                        TeamName = d.TeamName,
                        CarName = d.CarScreenName,
                        CarNameShort = d.CarScreenNameShort,
                        IRating = d.IRating,
                        License = d.LicString,
                        IsSpectator = d.IsSpectator != 0,
                        Laps = ds.Telemetry.CarIdxLap[idx],
                        LapsCompleted = ds.Telemetry.CarIdxLapCompleted[idx],
                        OnPitRoad = ds.Telemetry.CarIdxOnPitRoad[idx],
                        ClassPosition = ds.Telemetry.CarIdxClassPosition[idx],
                        Position = ds.Telemetry.CarIdxPosition[idx],
                        Location = location,
                        LastLapTime = lastLapTime,
                        BestLapTime = bestLapTime,
                        BestLapNum = bestLapNum,
                        LapDistPct = lapDistPct,
                        CarClassId = ds.SessionData.DriverInfo.Drivers.Where(a => a.CarIdx == idx).Select(a => a.CarClassID).FirstOrDefault()
                    };

                    if (Cars.TryGetValue(idx, out Car existingCar))
                    {
                        // If the car we just created, already exists in our existing cars dictionary,
                        // then reuse that instance instead of adding a new one
                        if (car.GetHashCode() == existingCar.GetHashCode())
                        {
                            car = existingCar;
                        }
                    }
                    else
                    {
                        Cars.Remove(idx);
                        Cars.Add(idx, car);
                    }

                    cars.Add(car);
                }

                state.Cars = cars.ToArray();
            }

            {
                List<Session> sessions = new List<Session>();

                foreach (var s in ds.SessionData.SessionInfo.Sessions)
                {
                    sessions.Add(new Session
                    {
                        LapsLimited = s.IsLimitedSessionLaps,
                        TimeLimited = s.IsLimitedTime,
                        TotalSessionTime = s._SessionTime / 10_000,
                        TotalSessionLaps = s._SessionLaps
                    });
                }

                state.Sessions = sessions.ToArray();
            }

            return state;
        }
    }
}