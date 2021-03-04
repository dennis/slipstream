#nullable enable

using System.Collections.Generic;
using static Slipstream.Components.IRacing.IIRacingEventFactory;
using Skies = Slipstream.Components.IRacing.IIRacingEventFactory.Skies;

namespace Slipstream.Components.IRacing.Plugins.GameState
{
    public class State : IState
    {
        public double SessionTime { get; set; }
        public int SessionNum { get; set; }
        public long DriverCarIdx { get; set; }
        public int DriverIncidentCount { get; set; }
        public Car[] Cars { get; set; } = new Car[0] { };
        public float FuelLevel { get; set; }
        public float LFtempCL { get; set; }
        public float LFtempCM { get; set; }
        public float LFtempCR { get; set; }
        public float RFtempCL { get; set; }
        public float RFtempCM { get; set; }
        public float RFtempCR { get; set; }
        public float LRtempCL { get; set; }
        public float LRtempCM { get; set; }
        public float LRtempCR { get; set; }
        public float RRtempCL { get; set; }
        public float RRtempCM { get; set; }
        public float RRtempCR { get; set; }
        public float LFwearL { get; set; }
        public float LFwearM { get; set; }
        public float LFwearR { get; set; }
        public float RFwearL { get; set; }
        public float RFwearM { get; set; }
        public float RFwearR { get; set; }
        public float LRwearL { get; set; }
        public float LRwearM { get; set; }
        public float LRwearR { get; set; }
        public float RRwearL { get; set; }
        public float RRwearM { get; set; }
        public float RRwearR { get; set; }
        public long TrackId { get; set; }
        public string TrackLength { get; set; } = string.Empty;
        public string TrackDisplayName { get; set; } = string.Empty;
        public string TrackCity { get; set; } = string.Empty;
        public string TrackCountry { get; set; } = string.Empty;
        public string TrackDisplayShortName { get; set; } = string.Empty;
        public string TrackConfigName { get; set; } = string.Empty;
        public string TrackType { get; set; } = string.Empty;
        public Skies Skies { get; set; } = Skies.Clear;
        public float TrackTempCrew { get; set; }
        public float AirTemp { get; set; }
        public float AirPressure { get; set; }
        public float RelativeHumidity { get; set; }
        public float FogLevel { get; set; }
        public IRacingCategoryEnum RaceCategory { get; set; } = IRacingCategoryEnum.Road;
        public SessionFlags SessionFlags { get; set; } = SessionFlags.None;
        public IRacingSessionStateEnum SessionState { get; set; } = IRacingSessionStateEnum.Invalid;
        public IRacingSessionTypeEnum SessionType { get; set; } = IRacingSessionTypeEnum.Practice;
        public ISession[] Sessions { get; set; } = new Session[] { };
        public ISession CurrentSession { get => Sessions[SessionNum]; }
        public int MyIncidentCount { get; internal set; }
        public int TeamIncidentCount { get; internal set; }

        public State()
        {
            Sessions = new Session[] { new Session() };
        }

        public IState Clone()
        {
            var cars = new List<Car>(Cars.Length);

            foreach (var c in Cars)
            {
                cars.Add(c.Clone());
            }

            var sessions = new List<ISession>(Sessions.Length);
            foreach (var s in Sessions)
            {
                sessions.Add(s);
            }

            return new State
            {
                SessionTime = SessionTime,
                SessionNum = SessionNum,
                DriverCarIdx = DriverCarIdx,
                TeamIncidentCount = TeamIncidentCount,
                MyIncidentCount = MyIncidentCount,
                DriverIncidentCount = DriverIncidentCount,
                Cars = cars.ToArray(),
                FuelLevel = FuelLevel,
                LFtempCL = LFtempCL,
                LFtempCM = LFtempCM,
                LFtempCR = LFtempCR,
                RFtempCL = RFtempCL,
                RFtempCM = RFtempCM,
                RFtempCR = RFtempCR,
                LRtempCL = LRtempCL,
                LRtempCM = LRtempCM,
                LRtempCR = LRtempCR,
                RRtempCL = RRtempCL,
                RRtempCM = RRtempCM,
                RRtempCR = RRtempCR,
                LFwearL = LFwearL,
                LFwearM = LFwearM,
                LFwearR = LFwearR,
                RFwearL = RFwearL,
                RFwearM = RFwearM,
                RFwearR = RFwearR,
                LRwearL = LRwearL,
                LRwearM = LRwearM,
                LRwearR = LRwearR,
                RRwearL = RRwearL,
                RRwearM = RRwearM,
                RRwearR = RRwearR,
                TrackId = TrackId,
                TrackLength = TrackLength,
                TrackDisplayName = TrackDisplayName,
                TrackCity = TrackCity,
                TrackCountry = TrackCountry,
                TrackDisplayShortName = TrackDisplayShortName,
                TrackConfigName = TrackConfigName,
                TrackType = TrackType,
                Skies = Skies,
                TrackTempCrew = TrackTempCrew,
                AirTemp = AirTemp,
                AirPressure = AirPressure,
                RelativeHumidity = RelativeHumidity,
                FogLevel = FogLevel,
                RaceCategory = RaceCategory,
                SessionFlags = SessionFlags,
                SessionState = SessionState,
                SessionType = SessionType,
                Sessions = sessions.ToArray(),
            };
        }
    }
}