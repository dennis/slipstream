#nullable enable

using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.FileMonitor.Events
{
    public class FileMonitorFileChanged : IEvent
    {
        public string EventType => "FileMonitorFileChanged";
        public ulong Uptime { get; set; }
        public string InstanceId { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            return obj is FileMonitorFileChanged changed &&
                   EventType == changed.EventType &&
                   InstanceId == changed.InstanceId &&
                   FilePath == changed.FilePath;
        }

        public override int GetHashCode()
        {
            int hashCode = 1499696410;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(InstanceId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FilePath);
            return hashCode;
        }
    }
}