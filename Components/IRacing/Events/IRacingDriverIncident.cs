#nullable enable

using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.IRacing.Events
{
    public class IRacingDriverIncident : IEvent
    {
        public string EventType => "IRacingDriverIncident";
        public ulong Uptime { get; set; }
        public int DriverIncidentCount { get; set; }
        public int DriverIncidentDelta { get; set; }
        public int TeamIncidentCount { get; set; }
        public int TeamIncidentDelta { get; set; }
        public int MyIncidentCount { get; set; }
        public int MyIncidentDelta { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is IRacingDriverIncident incident &&
                   EventType == incident.EventType &&
                   DriverIncidentCount == incident.DriverIncidentCount &&
                   DriverIncidentDelta == incident.DriverIncidentDelta &&
                   TeamIncidentCount == incident.TeamIncidentCount &&
                   TeamIncidentDelta == incident.TeamIncidentDelta &&
                   MyIncidentCount == incident.MyIncidentCount &&
                   MyIncidentDelta == incident.MyIncidentDelta;
        }

        public override int GetHashCode()
        {
            int hashCode = 1200671587;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + DriverIncidentCount.GetHashCode();
            hashCode = hashCode * -1521134295 + DriverIncidentDelta.GetHashCode();
            hashCode = hashCode * -1521134295 + TeamIncidentCount.GetHashCode();
            hashCode = hashCode * -1521134295 + TeamIncidentDelta.GetHashCode();
            hashCode = hashCode * -1521134295 + MyIncidentCount.GetHashCode();
            hashCode = hashCode * -1521134295 + MyIncidentDelta.GetHashCode();
            return hashCode;
        }
    }
}