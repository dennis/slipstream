using iRacingSDK;

namespace Slipstream.Components.IRacing.Plugins.GameState
{
    internal class Session : ISession
    {
        public bool LapsLimited { get; }
        public bool TimeLimited { get; }
        public double TotalSessionTime { get; }
        public int TotalSessionLaps { get; }

        public Session()
        {
        }

        public Session(SessionData._SessionInfo._Sessions s)
        {
            LapsLimited = s.IsLimitedSessionLaps;
            TimeLimited = s.IsLimitedTime;
            TotalSessionTime = s._SessionTime / 10_000;
            TotalSessionLaps = s._SessionLaps;
        }
    }
}