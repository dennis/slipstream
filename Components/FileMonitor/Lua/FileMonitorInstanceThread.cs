#nullable enable

using Serilog;
using Slipstream.Shared;
using Slipstream.Shared.Lua;
using System.Collections.Generic;
using System.IO;

namespace Slipstream.Components.FileMonitor.Lua
{
    public class FileMonitorInstanceThread : BaseInstanceThread, IFileMonitorInstanceThread
    {
        private readonly FileSystemWatcher[] Watchers = new FileSystemWatcher[] { };
        private readonly IEventBus EventBus;
        private readonly IFileMonitorEventFactory EventFactory;
        private readonly IEventBusSubscription Subscription;
        private readonly IEventHandlerController EventHandlerController;

        public FileMonitorInstanceThread(string instanceId, string[] paths, IEventBus eventBus, IFileMonitorEventFactory eventFactory, IEventBusSubscription eventBusSubscription, IEventHandlerController eventHandlerController, ILogger logger) : base(instanceId, logger)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
            Subscription = eventBusSubscription;
            EventHandlerController = eventHandlerController;

            var watchers = new List<FileSystemWatcher>();

            foreach (var path in paths)
            {
                var watcher = new FileSystemWatcher(path);
                watcher.Created += (_, e) => WatcherOnCreated(e);
                watcher.Changed += (_, e) => WatcherOnChanged(e);
                watcher.Deleted += (_, e) => WatcherOnDeleted(e);
                watcher.Renamed += (_, e) => WatcherOnRenamed(e);
                watcher.EnableRaisingEvents = true;

                watchers.Add(watcher);
            }

            Watchers = watchers.ToArray();
        }

        protected override void Main()
        {
            var fileMonitor = EventHandlerController.Get<EventHandler.FileMonitor>();
            fileMonitor.OnFileMonitorCommandScan += (s, e) => ScanExistingFiles();
            var internalEventHandler = EventHandlerController.Get<Internal.EventHandler.Internal>();
            internalEventHandler.OnInternalShutdown += (_, _e) => Stopping = true;

            Logger.Debug($"Starting {nameof(FileMonitorInstanceThread)} {InstanceId}");

            while (!Stopping)
            {
                IEvent? @event = Subscription.NextEvent(100);

                if (@event != null)
                {
                    EventHandlerController.HandleEvent(@event);
                }
            }

            Logger.Debug($"Stopping {nameof(FileMonitorInstanceThread)} {InstanceId}");
        }

        private void WatcherOnRenamed(RenamedEventArgs e)
        {
            EventBus.PublishEvent(EventFactory.CreateFileMonitorFileRenamed(InstanceId, e.FullPath, e.OldFullPath));
        }

        private void WatcherOnDeleted(FileSystemEventArgs e)
        {
            EventBus.PublishEvent(EventFactory.CreateFileMonitorFileDeleted(InstanceId, e.FullPath));
        }

        private void WatcherOnChanged(FileSystemEventArgs e)
        {
            EventBus.PublishEvent(EventFactory.CreateFileMonitorFileChanged(InstanceId, e.FullPath));
        }

        private void WatcherOnCreated(FileSystemEventArgs e)
        {
            EventBus.PublishEvent(EventFactory.CreateFileMonitorFileCreated(InstanceId, e.FullPath));
        }

        new public void Dispose()
        {
            base.Dispose();

            foreach (var w in Watchers)
            {
                w.Dispose();
            }
        }

        private void ScanExistingFiles()
        {
            foreach (var watcher in Watchers)
            {
                foreach (var path in Directory.GetFiles(watcher.Path, "*.*"))
                {
                    EventBus.PublishEvent(EventFactory.CreateFileMonitorFileCreated(InstanceId, path));
                }
            }

            EventBus.PublishEvent(EventFactory.CreateFileMonitorScanCompleted(InstanceId));
        }
    }
}