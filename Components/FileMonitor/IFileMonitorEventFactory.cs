#nullable enable

using Slipstream.Components.FileMonitor.Events;

namespace Slipstream.Components.FileMonitor
{
    public interface IFileMonitorEventFactory
    {
        FileMonitorFileChanged CreateFileMonitorFileChanged(string filePath);

        FileMonitorFileCreated CreateFileMonitorFileCreated(string path);

        FileMonitorFileDeleted CreateFileMonitorFileDeleted(string filePath);

        FileMonitorFileRenamed CreateFileMonitorFileRenamed(string filePath, string oldFilePath);

        FileMonitorCommandScan CreateFileMonitorCommandScan();
    }
}