using System.Collections.Generic;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingPractice : IIRacingSessionState
    {
        public string EventType => "IRacingPractice";
        public bool ExcludeFromTxrx => false;
        public ulong Uptime { get; set; }
        public double SessionTime { get; set; }
        public bool LapsLimited { get; set; }
        public bool TimeLimited { get; set; }
        public double TotalSessionTime { get; set; }
        public int TotalSessionLaps { get; set; }
        public string State { get; set; } = string.Empty; // Checkered, CoolDown, GetInChar, ParadeLaps, Racing, Warmup
        public string Category { get; set; } = string.Empty; // Road, Oval, DirtRoad, DirtOval

        public override bool Equals(object obj)
        {
            return obj is IRacingPractice practice &&
                   EventType == practice.EventType &&
                   ExcludeFromTxrx == practice.ExcludeFromTxrx &&
                   SessionTime == practice.SessionTime &&
                   LapsLimited == practice.LapsLimited &&
                   TimeLimited == practice.TimeLimited &&
                   TotalSessionTime == practice.TotalSessionTime &&
                   TotalSessionLaps == practice.TotalSessionLaps &&
                   State == practice.State &&
                   Category == practice.Category;
        }

        public override int GetHashCode()
        {
            int hashCode = -1762114191;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + SessionTime.GetHashCode();
            hashCode = hashCode * -1521134295 + LapsLimited.GetHashCode();
            hashCode = hashCode * -1521134295 + TimeLimited.GetHashCode();
            hashCode = hashCode * -1521134295 + TotalSessionTime.GetHashCode();
            hashCode = hashCode * -1521134295 + TotalSessionLaps.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(State);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Category);
            return hashCode;
        }
    }
}