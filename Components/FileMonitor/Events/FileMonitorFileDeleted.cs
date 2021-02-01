#nullable enable

using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.FileMonitor.Events
{
    public class FileMonitorFileDeleted : IEvent
    {
        public string EventType => "FileMonitorFileDeleted";
        public bool ExcludeFromTxrx => true;
        public ulong Uptime { get; set; }
        public string? FilePath { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is FileMonitorFileDeleted deleted &&
                   EventType == deleted.EventType &&
                   ExcludeFromTxrx == deleted.ExcludeFromTxrx &&
                   FilePath == deleted.FilePath;
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