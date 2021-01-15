#nullable enable

using System.Collections.Generic;

namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingDriverIncident : IEvent
    {
        public string EventType => "IRacingDriverIncident";
        public bool ExcludeFromTxrx => false;
        public ulong Uptime { get; set; }
        public int IncidentCount { get; set; }
        public int IncidentDelta { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is IRacingDriverIncident incident &&
                   EventType == incident.EventType &&
                   ExcludeFromTxrx == incident.ExcludeFromTxrx &&
                   IncidentCount == incident.IncidentCount &&
                   IncidentDelta == incident.IncidentDelta;
        }

        public override int GetHashCode()
        {
            int hashCode = 1200671587;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + IncidentCount.GetHashCode();
            hashCode = hashCode * -1521134295 + IncidentDelta.GetHashCode();
            return hashCode;
        }
    }
}