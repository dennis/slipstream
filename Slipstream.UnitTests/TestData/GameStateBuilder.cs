using Slipstream.Components.IRacing;
using Slipstream.Components.IRacing.Plugins.GameState;
using System;
using static Slipstream.Components.IRacing.IIRacingEventFactory;

namespace Slipstream.UnitTests.TestData
{
    public class GameStateBuilder
    {
        private readonly State State;

        public GameStateBuilder()
        {
            State = new State
            {
                SessionTime = 5123,
                SessionNum = 3,
                Cars = new Car[] { },
                TrackLength = "3.34 km",
                TrackDisplayName = "Test Track",
                TrackCity = "Test City",
                TrackCountry = "Test Contry",
                TrackDisplayShortName = "Test",
                TrackConfigName = "TrackConfig",
                TrackType = "Race",
                Skies = Skies.Clear,
                TrackTempCrew = 25.1f,
                AirTemp = 20.3f,
                AirPressure = 34,
                RaceCategory = IIRacingEventFactory.IRacingCategoryEnum.Road,
                SessionFlags = SessionFlags.None,
                SessionState = IIRacingEventFactory.IRacingSessionStateEnum.Racing,
                SessionType = IIRacingEventFactory.IRacingSessionTypeEnum.Race,
                Sessions = new Session[]
                {
                    new Session()
                    {
                        LapsLimited = true,
                        TimeLimited = false,
                    }
                },
            };
        }

        public IState SetupState(Action<State> a)
        {
            a(State);

            return State.Clone();
        }
    }
}