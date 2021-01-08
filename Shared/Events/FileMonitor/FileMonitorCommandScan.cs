#nullable enable

using System.Collections.Generic;

namespace Slipstream.Shared.Events.FileMonitor
{
    public class FileMonitorCommandScan : IEvent
    {
        public string EventType => "FileMonitorCommandScan";
        public bool ExcludeFromTxrx => true;

        public override bool Equals(object? obj)
        {
            return obj is FileMonitorCommandScan scan &&
                   EventType == scan.EventType &&
                   ExcludeFromTxrx == scan.ExcludeFromTxrx;
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
