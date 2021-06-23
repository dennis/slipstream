#nullable enable

using Slipstream.Components.FileMonitor.Events;
using Slipstream.Shared;

namespace Slipstream.Components.FileMonitor
{
    public interface IFileMonitorEventFactory
    {
        FileMonitorFileChanged CreateFileMonitorFileChanged(IEventEnvelope envelope, string filePath);

        FileMonitorFileCreated CreateFileMonitorFileCreated(IEventEnvelope envelope, string path);

        FileMonitorFileDeleted CreateFileMonitorFileDeleted(IEventEnvelope envelope, string filePath);

        FileMonitorFileRenamed CreateFileMonitorFileRenamed(IEventEnvelope envelope, string filePath, string oldFilePath);

        FileMonitorCommandScan CreateFileMonitorCommandScan(IEventEnvelope envelope);

        FileMonitorScanCompleted CreateFileMonitorScanCompleted(IEventEnvelope envelope);
    }
}