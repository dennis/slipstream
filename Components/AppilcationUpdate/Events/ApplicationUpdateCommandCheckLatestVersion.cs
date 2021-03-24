using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.AppilcationUpdate.Events
{
    public class ApplicationUpdateCommandCheckLatestVersion : IEvent
    {
        public string EventType => nameof(ApplicationUpdateCommandCheckLatestVersion);

        public bool ExcludeFromTxrx => true;

        public ulong Uptime { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ApplicationUpdateCommandCheckLatestVersion version &&
                   EventType == version.EventType &&
                   ExcludeFromTxrx == version.ExcludeFromTxrx;
        }

        public override int GetHashCode()
        {
            int hashCode = -441302714;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            return hashCode;
        }
    }
}
