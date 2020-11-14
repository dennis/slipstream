using System;

namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingCurrentSession : IEvent, IEquatable<IRacingCurrentSession>
    {
        public enum SessionTypes
        {
            Practice,
            OpenQualify,
            Race,
            Warmup,
            LoneQualify,
            OfflineTesting
        }

        public string EventType => "IRacingCurrentSession";
        public SessionTypes SessionType { get; set; } = SessionTypes.OfflineTesting;
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