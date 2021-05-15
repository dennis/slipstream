#nullable enable

using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.FileMonitor.Events
{
    public class FileMonitorFileRenamed : IEvent
    {
        public string EventType => "FileMonitorFileRenamed";
        public ulong Uptime { get; set; }
        public string InstanceId { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string OldFilePath { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            return obj is FileMonitorFileRenamed renamed &&
                   EventType == renamed.EventType &&
                   InstanceId == renamed.InstanceId &&
                   FilePath == renamed.FilePath &&
                   OldFilePath == renamed.OldFilePath;
        }

        public override int GetHashCode()
        {
            int hashCode = 1615632865;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(InstanceId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FilePath);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(OldFilePath);
            return hashCode;
        }
    }
}