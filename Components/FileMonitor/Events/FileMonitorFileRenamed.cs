﻿#nullable enable

using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.FileMonitor.Events
{
    public class FileMonitorFileRenamed : IEvent
    {
        public string EventType => "FileMonitorFileRenamed";
        public bool ExcludeFromTxrx => true;
        public ulong Uptime { get; set; }
        public string? FilePath { get; set; }
        public string? OldFilePath { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is FileMonitorFileRenamed renamed &&
                   EventType == renamed.EventType &&
                   ExcludeFromTxrx == renamed.ExcludeFromTxrx &&
                   FilePath == renamed.FilePath &&
                   OldFilePath == renamed.OldFilePath;
        }

        public override int GetHashCode()
        {
            int hashCode = 1615632865;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string?>.Default.GetHashCode(FilePath);
            hashCode = hashCode * -1521134295 + EqualityComparer<string?>.Default.GetHashCode(OldFilePath);
            return hashCode;
        }
    }
}