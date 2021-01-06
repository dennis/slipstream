#nullable enable

using System.Collections.Generic;

namespace Slipstream.Shared.Events.FileMonitor
{
    public class FileMonitorScanCompleted : IEvent
    {
        public string EventType => "FileMonitorScanCompleted";
        public bool ExcludeFromTxrx => true;

        public override bool Equals(object? obj)
        {
            return obj is FileMonitorScanCompleted completed &&
                   EventType == completed.EventType &&
                   ExcludeFromTxrx == completed.ExcludeFromTxrx;
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
