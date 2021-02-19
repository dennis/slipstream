using Slipstream.Components.IRacing;
using Slipstream.Components.IRacing.Plugins.GameState;
using System.Collections.Generic;
using static Slipstream.Components.IRacing.IIRacingEventFactory;

namespace Slipstream.UnitTests.TestData
{
    public class TestState : IState
    {
        public double SessionTime { get; set; } = 5123;

        public int SessionNum { get; set; } = 3;

        public long DriverCarIdx { get; set; } = 0;

        public int DriverIncidentCount { get; set; } = 0;

        public Car[] Cars { get; set; } = new Car[] { };

        public float FuelLevel { get; set; } = 0;

        public float LFtempCL { get; set; } = 0;

        public float LFtempCM { get; set; } = 0;

        public float LFtempCR { get; set; } = 0;

        public float RFtempCL { get; set; } = 0;

        public float RFtempCM { get; set; } = 0;

        public float RFtempCR { get; set; } = 0;

        public float LRtempCL { get; set; } = 0;

        public float LRtempCM { get; set; } = 0;

        public float LRtempCR { get; set; } = 0;

        public float RRtempCL { get; set; } = 0;

        public float RRtempCM { get; set; } = 0;

        public float RRtempCR { get; set; } = 0;

        public float LFwearL { get; set; } = 100;

        public float LFwearM { get; set; } = 99;

        public float LFwearR { get; set; } = 98;

        public float RFwearL { get; set; } = 97;

        public float RFwearM { get; set; } = 96;

        public float RFwearR { get; set; } = 95;

        public float LRwearL { get; set; } = 94;

        public float LRwearM { get; set; } = 93;

        public float LRwearR { get; set; } = 92;

        public float RRwearL { get; set; } = 91;

        public float RRwearM { get; set; } = 90;

        public float RRwearR { get; set; } = 90;

        public long TrackId { get; set; } = 1;

        public string TrackLength { get; set; } = "3.34 km";

        public string TrackDisplayName { get; set; } = "Test Track";

        public string TrackCity { get; set; } = "Test City";

        public string TrackCountry { get; set; } = "Test Contry";

        public string TrackDisplayShortName { get; set; } = "Test";

        public string TrackConfigName { get; set; } = "TrackConfig";

        public string TrackType { get; set; } = "Race";

        public Skies Skies { get; set; } = Skies.Clear;

        public float TrackTempCrew { get; set; } = 25.1f;

        public float AirTemp { get; set; } = 20.3f;

        public float AirPressure { get; set; } = 34;

        public float RelativeHumidity { get; set; } = 0;

        public float FogLevel { get; set; } = 0;

        public IIRacingEventFactory.IRacingCategoryEnum RaceCategory { get; set; } = IIRacingEventFactory.IRacingCategoryEnum.Road;

        public SessionFlags SessionFlags { get; set; } = SessionFlags.None;

        public IIRacingEventFactory.IRacingSessionStateEnum SessionState { get; set; } = IIRacingEventFactory.IRacingSessionStateEnum.Racing;

        public IIRacingEventFactory.IRacingSessionTypeEnum SessionType { get; set; } = IIRacingEventFactory.IRacingSessionTypeEnum.Race;

        public ISession[] Sessions { get; set; } = new Session[] { };

        public ISession CurrentSession { get => Sessions[0]; }

        public float LastLapTime { get; set; }

        public TestState()
        {
            var list = new List<ISession>
                {
                    new TestSession()
                    {
                        LapsLimited = true,
                        TimeLimited = false,
                    }
                };

            Sessions = list.ToArray();
        }
    }
}