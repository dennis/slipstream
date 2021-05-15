namespace Slipstream.Components.IRacing.GameState
{
    internal class Session : ISession
    {
        public bool LapsLimited { get; set; }
        public bool TimeLimited { get; set; }
        public double TotalSessionTime { get; set; }
        public int TotalSessionLaps { get; set; }

        public Session Clone()
        {
            return new Session
            {
                LapsLimited = LapsLimited,
                TimeLimited = TimeLimited,
                TotalSessionTime = TotalSessionTime,
                TotalSessionLaps = TotalSessionLaps,
            };
        }
    }
}