using Slipstream.Shared;
using Slipstream.Shared.Events.FileMonitor;
using Slipstream.Shared.Factories;
using System.Collections.Generic;
using System.IO;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    internal class FileMonitorPlugin : BasePlugin
    {
        private readonly IFileMonitorEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private readonly IList<FileSystemWatcher> fileSystemWatchers = new List<FileSystemWatcher>();

        public FileMonitorPlugin(string id, IFileMonitorEventFactory eventFactory, IEventBus eventBus, IFileMonitorConfiguration fileMonitorConfiguration) : base(id, "FileMonitorPlugin", "FileMonitorPlugin", "Core", true)
        {
            EventFactory = eventFactory;
            EventBus = eventBus;

            var FileMonitor = EventHandler.Get<Slipstream.Shared.EventHandlers.FileMonitor>();
            FileMonitor.OnFileMonitorCommandScan += (s, e) => ScanExistingFiles();

            StartMonitoring(fileMonitorConfiguration.FileMonitorPaths);

            ScanExistingFiles();
        }

        private void ScanExistingFiles()
        {
            foreach (var watcher in fileSystemWatchers)
            {
                foreach (var path in Directory.GetFiles(watcher.Path, "*.*"))
                {
                    EventBus.PublishEvent(EventFactory.CreateFileMonitorFileCreated(path));
                }
            }

            EventBus.PublishEvent(new FileMonitorScanCompleted());
        }

        private void StartMonitoring(string[] paths)
        {
            // Delete all watchers, and then recreate them
            foreach (var watcher in fileSystemWatchers)
                watcher.Dispose();
            fileSystemWatchers.Clear();

            foreach (var path in paths)
            {
                var watcher = new FileSystemWatcher(path);
                watcher.Created += WatcherOnCreated;
                watcher.Changed += WatcherOnChanged;
                watcher.Deleted += WatcherOnDeleted;
                watcher.Renamed += WatcherOnRenamed;
                watcher.EnableRaisingEvents = true;

                fileSystemWatchers.Add(watcher);
            }
        }

        private void WatcherOnRenamed(object sender, RenamedEventArgs e)
        {
            EventBus.PublishEvent(EventFactory.CreateFileMonitorFileRenamed(e.FullPath, e.OldFullPath));
        }

        private void WatcherOnDeleted(object sender, FileSystemEventArgs e)
        {
            EventBus.PublishEvent(EventFactory.CreateFileMonitorFileDeleted(e.FullPath));
        }

        private void WatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            EventBus.PublishEvent(EventFactory.CreateFileMonitorFileChanged(e.FullPath));
        }

        private void WatcherOnCreated(object sender, FileSystemEventArgs e)
        {
            EventBus.PublishEvent(EventFactory.CreateFileMonitorFileCreated(e.FullPath));
        }
    }
}