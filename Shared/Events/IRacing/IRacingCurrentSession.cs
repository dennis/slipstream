using System;

namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingCurrentSession : IEvent, IEquatable<IRacingCurrentSession>
    {
        public string EventType => "IRacingCurrentSession";
        public bool ExcludeFromTxrx => false;
        public string SessionType { get; set; } = ""; // Practice, Open Qualify, Lone Qualify, Offline Testing, Race, Warmup
        public bool TimeLimited { get; set; }
        public bool LapsLimited { get; set; }
        public int TotalSessionLaps { get; set; }
        public double TotalSessionTime { get; set; }

        public bool Equals(IRacingCurrentSession other)
        {
            return SessionType == other.SessionType &&
                TimeLimited == other.TimeLimited &&
                LapsLimited == other.LapsLimited &&
                TotalSessionLaps == other.TotalSessionLaps &&
                TotalSessionTime == other.TotalSessionTime;
        }
    }
}