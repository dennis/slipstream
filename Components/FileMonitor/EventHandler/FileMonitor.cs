#nullable enable

using Slipstream.Components.FileMonitor.Events;
using Slipstream.Shared;
using System;

namespace Slipstream.Components.FileMonitor.EventHandler
{
    internal class FileMonitor : IEventHandler
    {
        public event EventHandler<FileMonitorCommandScan>? OnFileMonitorCommandScan;

        public event EventHandler<FileMonitorFileChanged>? OnFileMonitorFileChanged;

        public event EventHandler<FileMonitorFileCreated>? OnFileMonitorFileCreated;

        public event EventHandler<FileMonitorFileDeleted>? OnFileMonitorFileDeleted;

        public event EventHandler<FileMonitorFileRenamed>? OnFileMonitorFileRenamed;

        public event EventHandler<FileMonitorScanCompleted>? OnFileMonitorScanCompleted;

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            return @event switch
            {
                FileMonitorCommandScan tev => OnEvent(OnFileMonitorCommandScan, tev),
                FileMonitorFileCreated tev => OnEvent(OnFileMonitorFileCreated, tev),
                FileMonitorFileChanged tev => OnEvent(OnFileMonitorFileChanged, tev),
                FileMonitorFileDeleted tev => OnEvent(OnFileMonitorFileDeleted, tev),
                FileMonitorFileRenamed tev => OnEvent(OnFileMonitorFileRenamed, tev),
                FileMonitorScanCompleted tev => OnEvent(OnFileMonitorScanCompleted, tev),
                _ => IEventHandler.HandledStatus.NotMine,
            };
        }

        private IEventHandler.HandledStatus OnEvent<TEvent>(EventHandler<TEvent>? onEvent, TEvent args)
        {
            if (onEvent != null)
            {
                onEvent.Invoke(this, args);
                return IEventHandler.HandledStatus.Handled;
            }
            return IEventHandler.HandledStatus.UseDefault;
        }
    }
}