using System;
using System.Collections.Generic;

namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingCurrentSession : IEvent
    {
        public string EventType => "IRacingCurrentSession";
        public bool ExcludeFromTxrx => false;
        public string Category { get; set; } = "";
        public string SessionType { get; set; } = "";
        public bool TimeLimited { get; set; }
        public bool LapsLimited { get; set; }
        public int TotalSessionLaps { get; set; }
        public double TotalSessionTime { get; set; }

        public override bool Equals(object obj)
        {
            return obj is IRacingCurrentSession session &&
                   EventType == session.EventType &&
                   ExcludeFromTxrx == session.ExcludeFromTxrx &&
                   Category == session.Category &&
                   SessionType == session.SessionType &&
                   TimeLimited == session.TimeLimited &&
                   LapsLimited == session.LapsLimited &&
                   TotalSessionLaps == session.TotalSessionLaps &&
                   TotalSessionTime == session.TotalSessionTime;
        }

        public override int GetHashCode()
        {
            int hashCode = 2106272073;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Category);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SessionType);
            hashCode = hashCode * -1521134295 + TimeLimited.GetHashCode();
            hashCode = hashCode * -1521134295 + LapsLimited.GetHashCode();
            hashCode = hashCode * -1521134295 + TotalSessionLaps.GetHashCode();
            hashCode = hashCode * -1521134295 + TotalSessionTime.GetHashCode();
            return hashCode;
        }
    }
}