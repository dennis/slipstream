using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.AppilcationUpdate.Events
{
    public class ApplicationUpdateLatestVersionChanged : IEvent
    {
        public string EventType => nameof(ApplicationUpdateLatestVersionChanged);

        public ulong Uptime { get; set; }

        public string LatestVersion { get; set; } = string.Empty;

        public override bool Equals(object obj)
        {
            return obj is ApplicationUpdateLatestVersionChanged changed &&
                   EventType == changed.EventType &&
                   LatestVersion == changed.LatestVersion;
        }

        public override int GetHashCode()
        {
            int hashCode = 1556716928;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LatestVersion);
            return hashCode;
        }
    }
}