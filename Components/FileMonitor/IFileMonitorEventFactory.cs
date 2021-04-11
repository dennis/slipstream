#nullable enable

using Slipstream.Components.FileMonitor.Events;

namespace Slipstream.Components.FileMonitor
{
    public interface IFileMonitorEventFactory
    {
        FileMonitorFileChanged CreateFileMonitorFileChanged(string instanceId, string filePath);

        FileMonitorFileCreated CreateFileMonitorFileCreated(string instanceId, string path);

        FileMonitorFileDeleted CreateFileMonitorFileDeleted(string instanceId, string filePath);

        FileMonitorFileRenamed CreateFileMonitorFileRenamed(string instanceId, string filePath, string oldFilePath);

        FileMonitorCommandScan CreateFileMonitorCommandScan(string instanceId);

        FileMonitorScanCompleted CreateFileMonitorScanCompleted(string instanceId);
    }
}