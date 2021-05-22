#nullable enable

using Slipstream.Components.FileMonitor.Events;

namespace Slipstream.Components.FileMonitor.EventFactory
{
    public class FileMonitorEventFactory : IFileMonitorEventFactory
    {
        public FileMonitorFileChanged CreateFileMonitorFileChanged(string instanceId, string filePath)
        {
            return new FileMonitorFileChanged { InstanceId = instanceId, FilePath = filePath };
        }

        public FileMonitorFileCreated CreateFileMonitorFileCreated(string instanceId, string path)
        {
            return new FileMonitorFileCreated { InstanceId = instanceId, FilePath = path };
        }

        public FileMonitorFileDeleted CreateFileMonitorFileDeleted(string instanceId, string filePath)
        {
            return new FileMonitorFileDeleted { InstanceId = instanceId, FilePath = filePath };
        }

        public FileMonitorFileRenamed CreateFileMonitorFileRenamed(string instanceId, string filePath, string oldFilePath)
        {
            return new FileMonitorFileRenamed { InstanceId = instanceId, FilePath = filePath, OldFilePath = oldFilePath };
        }

        public FileMonitorCommandScan CreateFileMonitorCommandScan(string instanceId)
        {
            return new FileMonitorCommandScan { InstanceId = instanceId };
        }

        public FileMonitorScanCompleted CreateFileMonitorScanCompleted(string instanceId)
        {
            return new FileMonitorScanCompleted();
        }
    }
}