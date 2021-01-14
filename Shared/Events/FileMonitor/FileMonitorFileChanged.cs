#nullable enable

using System.Collections.Generic;

namespace Slipstream.Shared.Events.FileMonitor
{
    public class FileMonitorFileChanged : IEvent
    {
        public string EventType => "FileMonitorFileChanged";
        public bool ExcludeFromTxrx => true;
        public ulong Uptime { get; set; }
        public string? FilePath { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is FileMonitorFileChanged changed &&
                   EventType == changed.EventType &&
                   ExcludeFromTxrx == changed.ExcludeFromTxrx &&
                   FilePath == changed.FilePath;
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
