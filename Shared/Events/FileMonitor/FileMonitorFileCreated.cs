#nullable enable

using System.Collections.Generic;

namespace Slipstream.Shared.Events.FileMonitor
{
    public class FileMonitorFileCreated : IEvent
    {
        public string EventType => "FileMonitorFileCreated";
        public bool ExcludeFromTxrx => true;
        public ulong Uptime { get; set; }
        public string? FilePath { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is FileMonitorFileCreated created &&
                   EventType == created.EventType &&
                   ExcludeFromTxrx == created.ExcludeFromTxrx &&
                   FilePath == created.FilePath;
        }

        public override int GetHashCode()
        {
            int hashCode = 1499696410;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string?>.Default.GetHashCode(FilePath);
            return hashCode;
        }
    }
}
