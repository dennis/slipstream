#nullable enable

using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.FileMonitor.Events
{
    public class FileMonitorCommandScan : IEvent
    {
        public string EventType => "FileMonitorCommandScan";
        public bool ExcludeFromTxrx => true;
        public ulong Uptime { get; set; }

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