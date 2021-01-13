#nullable enable

using Slipstream.Shared.Events.FileMonitor;
using static Slipstream.Shared.EventHandler;

namespace Slipstream.Shared.EventHandlers
{
    internal class FileMonitor : IEventHandler
    {
        private readonly EventHandler Parent;

        public FileMonitor(EventHandler eventHandler)
        {
            Parent = eventHandler;
        }

        public delegate void OnFileMonitorCommandScanHandler(EventHandler source, EventHandlerArgs<FileMonitorCommandScan> e);

        public delegate void OnFileMonitorFileChangedHandler(EventHandler source, EventHandlerArgs<FileMonitorFileChanged> e);

        public delegate void OnFileMonitorFileCreatedHandler(EventHandler source, EventHandlerArgs<FileMonitorFileCreated> e);

        public delegate void OnFileMonitorFileDeletedHandler(EventHandler source, EventHandlerArgs<FileMonitorFileDeleted> e);

        public delegate void OnFileMonitorFileRenamedHandler(EventHandler source, EventHandlerArgs<FileMonitorFileRenamed> e);

        public delegate void OnFileMonitorScanCompletedHandler(EventHandler source, EventHandlerArgs<FileMonitorScanCompleted> e);

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