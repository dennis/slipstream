using Slipstream.Shared.Events.FileMonitor;

#nullable enable

namespace Slipstream.Shared.Factories
{
    public class FileMonitorEventFactory : IFileMonitorEventFactory
    {
        public FileMonitorFileChanged CreateFileMonitorFileChanged(string filePath)
        {
            return new FileMonitorFileChanged { FilePath = filePath };
        }

        public FileMonitorFileCreated CreateFileMonitorFileCreated(string path)
        {
            return new FileMonitorFileCreated { FilePath = path };
        }

        public FileMonitorFileDeleted CreateFileMonitorFileDeleted(string filePath)
        {
            return new FileMonitorFileDeleted { FilePath = filePath };
        }

        public FileMonitorFileRenamed CreateFileMonitorFileRenamed(string filePath, string oldFilePath)
        {
            return new FileMonitorFileRenamed { FilePath = filePath, OldFilePath = oldFilePath };
        }

        public FileMonitorCommandScan CreateFileMonitorCommandScan()
        {
            return new FileMonitorCommandScan();
        }
    }
}
