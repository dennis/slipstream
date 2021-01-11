using Slipstream.Shared.Events.FileMonitor;

#nullable enable

namespace Slipstream.Shared.Factories
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
