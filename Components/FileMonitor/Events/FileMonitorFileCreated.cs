#nullable enable

using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.FileMonitor.Events
{
    public class FileMonitorFileCreated : IEvent
    {
        public string EventType => "FileMonitorFileCreated";
        public bool ExcludeFromTxrx => true;
        public ulong Uptime { get; set; }
        public string InstanceId { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            return obj is FileMonitorFileCreated created &&
                   EventType == created.EventType &&
                   ExcludeFromTxrx == created.ExcludeFromTxrx &&
                   InstanceId == created.InstanceId &&
                   FilePath == created.FilePath;
        }

        public override int GetHashCode()
        {
            int hashCode = 1499696410;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(InstanceId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FilePath);
            return hashCode;
        }
    }
}