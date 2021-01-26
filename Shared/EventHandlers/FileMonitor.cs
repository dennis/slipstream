#nullable enable

using Slipstream.Shared.Events.FileMonitor;

namespace Slipstream.Shared.EventHandlers
{
    internal class FileMonitor : IEventHandler
    {
        private readonly IEventHandlerController Parent;

        public FileMonitor(IEventHandlerController eventHandler)
        {
            Parent = eventHandler;
        }

        public delegate void OnFileMonitorCommandScanHandler(IEventHandlerController source, EventHandlerArgs<FileMonitorCommandScan> e);

        public delegate void OnFileMonitorFileChangedHandler(IEventHandlerController source, EventHandlerArgs<FileMonitorFileChanged> e);

        public delegate void OnFileMonitorFileCreatedHandler(IEventHandlerController source, EventHandlerArgs<FileMonitorFileCreated> e);

        public delegate void OnFileMonitorFileDeletedHandler(IEventHandlerController source, EventHandlerArgs<FileMonitorFileDeleted> e);

        public delegate void OnFileMonitorFileRenamedHandler(IEventHandlerController source, EventHandlerArgs<FileMonitorFileRenamed> e);

        public delegate void OnFileMonitorScanCompletedHandler(IEventHandlerController source, EventHandlerArgs<FileMonitorScanCompleted> e);

        public event OnFileMonitorCommandScanHandler? OnFileMonitorCommandScan;

        public event OnFileMonitorFileChangedHandler? OnFileMonitorFileChanged;

        public event OnFileMonitorFileCreatedHandler? OnFileMonitorFileCreated;

        public event OnFileMonitorFileDeletedHandler? OnFileMonitorFileDeleted;

        public event OnFileMonitorFileRenamedHandler? OnFileMonitorFileRenamed;

        public event OnFileMonitorScanCompletedHandler? OnFileMonitorScanCompleted;

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            switch (@event)
            {
                case FileMonitorCommandScan tev:
                    if (OnFileMonitorCommandScan != null)
                    {
                        OnFileMonitorCommandScan.Invoke(Parent, new EventHandlerArgs<FileMonitorCommandScan>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case FileMonitorFileCreated tev:
                    if (OnFileMonitorFileCreated != null)
                    {
                        OnFileMonitorFileCreated.Invoke(Parent, new EventHandlerArgs<FileMonitorFileCreated>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case FileMonitorFileChanged tev:
                    if (OnFileMonitorFileChanged != null)
                    {
                        OnFileMonitorFileChanged.Invoke(Parent, new EventHandlerArgs<FileMonitorFileChanged>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case FileMonitorFileDeleted tev:
                    if (OnFileMonitorFileDeleted != null)
                    {
                        OnFileMonitorFileDeleted.Invoke(Parent, new EventHandlerArgs<FileMonitorFileDeleted>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case FileMonitorFileRenamed tev:
                    if (OnFileMonitorFileRenamed != null)
                    {
                        OnFileMonitorFileRenamed.Invoke(Parent, new EventHandlerArgs<FileMonitorFileRenamed>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case FileMonitorScanCompleted tev:
                    if (OnFileMonitorScanCompleted != null)
                    {
                        OnFileMonitorScanCompleted.Invoke(Parent, new EventHandlerArgs<FileMonitorScanCompleted>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
            }

            return IEventHandler.HandledStatus.NotMine;
        }
    }
}