#nullable enable

using Slipstream.Components.FileMonitor.Events;
using Slipstream.Shared;

namespace Slipstream.Components.FileMonitor.EventFactory
{
    public class FileMonitorEventFactory : IFileMonitorEventFactory
    {
        public FileMonitorFileChanged CreateFileMonitorFileChanged(IEventEnvelope envelope, string filePath)
        {
            return new FileMonitorFileChanged { Envelope = envelope.Clone(), FilePath = filePath };
        }

        public FileMonitorFileCreated CreateFileMonitorFileCreated(IEventEnvelope envelope, string path)
        {
            return new FileMonitorFileCreated { Envelope = envelope.Clone(), FilePath = path };
        }

        public FileMonitorFileDeleted CreateFileMonitorFileDeleted(IEventEnvelope envelope, string filePath)
        {
            return new FileMonitorFileDeleted { Envelope = envelope.Clone(), FilePath = filePath };
        }

        public FileMonitorFileRenamed CreateFileMonitorFileRenamed(IEventEnvelope envelope, string filePath, string oldFilePath)
        {
            return new FileMonitorFileRenamed { Envelope = envelope.Clone(), FilePath = filePath, OldFilePath = oldFilePath };
        }

        public FileMonitorCommandScan CreateFileMonitorCommandScan(IEventEnvelope envelope)
        {
            return new FileMonitorCommandScan { Envelope = envelope.Clone() };
        }

        public FileMonitorScanCompleted CreateFileMonitorScanCompleted(IEventEnvelope envelope)
        {
            return new FileMonitorScanCompleted { Envelope = envelope.Clone() };
        }
    }
}