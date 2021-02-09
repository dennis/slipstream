#nullable enable

using iRacingSDK;
using System;
using System.Collections.Generic;
using static Slipstream.Components.IRacing.IIRacingEventFactory;
using Skies = Slipstream.Components.IRacing.IIRacingEventFactory.Skies;

namespace Slipstream.Components.IRacing.Plugins.GameState
{
    internal class State : IState
    {
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

        public double SessionTime { get; }
        public int SessionNum { get; }
        public long DriverCarIdx { get; }
        public int DriverIncidentCount { get; }
        public Car[] Cars { get; } = new Car[0] { };
        public float FuelLevel { get; }
        public float LFtempCL { get; }
        public float LFtempCM { get; }
        public float LFtempCR { get; }
        public float RFtempCL { get; }
        public float RFtempCM { get; }
        public float RFtempCR { get; }
        public float LRtempCL { get; }
        public float LRtempCM { get; }
        public float LRtempCR { get; }
        public float RRtempCL { get; }
        public float RRtempCM { get; }
        public float RRtempCR { get; }
        public float LFwearL { get; }
        public float LFwearM { get; }
        public float LFwearR { get; }
        public float RFwearL { get; }
        public float RFwearM { get; }
        public float RFwearR { get; }
        public float LRwearL { get; }
        public float LRwearM { get; }
        public float LRwearR { get; }
        public float RRwearL { get; }
        public float RRwearM { get; }
        public float RRwearR { get; }
        public long TrackId { get; }
        public string TrackLength { get; } = string.Empty;
        public string TrackDisplayName { get; } = string.Empty;
        public string TrackCity { get; } = string.Empty;
        public string TrackCountry { get; } = string.Empty;
        public string TrackDisplayShortName { get; } = string.Empty;
        public string TrackConfigName { get; } = string.Empty;
        public string TrackType { get; } = string.Empty;
        public Skies Skies { get; } = Skies.Clear;
        public float TrackTempCrew { get; }
        public float AirTemp { get; }
        public float AirPressure { get; }
        public float RelativeHumidity { get; }
        public float FogLevel { get; }
        public IRacingCategoryEnum RaceCategory { get; } = IRacingCategoryEnum.Road;
        public SessionFlags SessionFlags { get; } = SessionFlags.None;
        public IRacingSessionStateEnum SessionState { get; } = IRacingSessionStateEnum.Invalid;
        public IRacingSessionTypeEnum SessionType { get; } = IRacingSessionTypeEnum.Practice;
        public ISession[] Sessions { get; } = new Session[] { };
        public ISession CurrentSession { get => Sessions[SessionNum]; }

        public State()
        {
            Sessions = new Session[] { new Session() };
        }

        public State(DataSample ds)
        {
            SessionTime = ds.Telemetry.SessionTime;
            SessionNum = ds.Telemetry.SessionNum;
            SessionFlags = (SessionFlags)(int)ds.Telemetry.SessionFlags;
            SessionState = SessionStateMapping[ds.Telemetry.SessionState];
            SessionType = IRacingSessionTypes[ds.SessionData.SessionInfo.Sessions[ds.Telemetry.SessionNum].SessionType];

            DriverCarIdx = ds.SessionData.DriverInfo.DriverCarIdx;
            DriverIncidentCount = Convert.ToInt32(ds.Telemetry["PlayerCarDriverIncidentCount"]);
            FuelLevel = ds.Telemetry.FuelLevel;

            LFtempCL = ds.Telemetry.LFtempCL;
            LFtempCM = ds.Telemetry.LFtempCM;
            LFtempCR = ds.Telemetry.LFtempCR;

            RFtempCL = ds.Telemetry.RFtempCL;
            RFtempCM = ds.Telemetry.RFtempCM;
            RFtempCR = ds.Telemetry.RFtempCR;

            LRtempCL = ds.Telemetry.LRtempCL;
            LRtempCM = ds.Telemetry.LRtempCM;
            LRtempCR = ds.Telemetry.LRtempCR;

            RRtempCL = ds.Telemetry.RRtempCL;
            RRtempCM = ds.Telemetry.RRtempCM;
            RRtempCR = ds.Telemetry.RRtempCR;

            LFwearL = ds.Telemetry.LFwearL;
            LFwearM = ds.Telemetry.LFwearM;
            LFwearR = ds.Telemetry.LFwearR;

            RFwearL = ds.Telemetry.RFwearL;
            RFwearM = ds.Telemetry.RFwearM;
            RFwearR = ds.Telemetry.RFwearR;

            LRwearL = ds.Telemetry.LRwearL;
            LRwearM = ds.Telemetry.LRwearM;
            LRwearR = ds.Telemetry.LRwearR;

            RRwearL = ds.Telemetry.RRwearL;
            RRwearM = ds.Telemetry.RRwearM;
            RRwearR = ds.Telemetry.RRwearR;

            TrackId = ds.SessionData.WeekendInfo.TrackID;
            TrackLength = ds.SessionData.WeekendInfo.TrackLength;
            TrackDisplayName = ds.SessionData.WeekendInfo.TrackDisplayName;
            TrackCity = ds.SessionData.WeekendInfo.TrackCity;
            TrackCountry = ds.SessionData.WeekendInfo.TrackCountry;
            TrackDisplayShortName = ds.SessionData.WeekendInfo.TrackDisplayShortName;
            TrackConfigName = ds.SessionData.WeekendInfo.TrackConfigName;
            TrackType = ds.SessionData.WeekendInfo.TrackType;

            Skies = (Skies)(int)ds.Telemetry.Skies;
            TrackTempCrew = ds.Telemetry.TrackTempCrew;
            AirTemp = ds.Telemetry.AirTemp;
            AirPressure = ds.Telemetry.AirPressure;
            RelativeHumidity = ds.Telemetry.RelativeHumidity;
            FogLevel = ds.Telemetry.FogLevel;

            RaceCategory = IRacingCategoryTypes[ds.SessionData.WeekendInfo.Category];

            {
                List<Car> cars = new List<Car>();
                foreach (var d in ds.SessionData.DriverInfo.Drivers)
                {
                    cars.Add(new Car((int)d.CarIdx, ds));
                }

                Cars = cars.ToArray();
            }

            {
                List<Session> sessions = new List<Session>();

                foreach (var s in ds.SessionData.SessionInfo.Sessions)
                {
                    sessions.Add(new Session(s));
                }

                Sessions = sessions.ToArray();
            }
        }
    }
}