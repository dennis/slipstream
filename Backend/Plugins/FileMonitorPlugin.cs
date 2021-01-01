using Slipstream.Shared;
using Slipstream.Shared.Events.FileMonitor;
using Slipstream.Shared.Events.Setting;
using System.Collections.Generic;
using System.IO;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    class FileMonitorPlugin : BasePlugin
    {
        private readonly IEventBus EventBus;
        private readonly IList<FileSystemWatcher> fileSystemWatchers = new List<FileSystemWatcher>();
        private readonly bool InitialScan = false;

        public FileMonitorPlugin(string id, IEventBus eventBus, FileMonitorSettings settings) : base(id, "FileMonitorPlugin", "FileMonitorPlugin", "Core")
        {
            EventBus = eventBus;

            OnFileMonitorSettings(settings);

            EventHandler.OnSettingFileMonitorSettings += (s, e) => OnFileMonitorSettings(e.Event);
        }

        public override void OnDisable()
        {
            UpdateWatchers();
        }

        private void UpdateWatchers()
        {
            foreach (var watcher in fileSystemWatchers)
                watcher.EnableRaisingEvents = Enabled;
        }

        public override void OnEnable()
        {
            UpdateWatchers();

            RescanExistingFiles();
        }

        private void RescanExistingFiles()
        {
            foreach (var watcher in fileSystemWatchers)
            {
                foreach (var path in Directory.GetFiles(watcher.Path, "*.*"))
                {
                    EventBus.PublishEvent(new FileMonitorFileCreated
                    {
                        FilePath = path
                    });
                }
            }

            EventBus.PublishEvent(new FileMonitorScanCompleted());
        }

        private void OnFileMonitorSettings(FileMonitorSettings ev)
        {
            // Delete all watchers, and then recreate them
            foreach (var watcher in fileSystemWatchers)
                watcher.Dispose();
            fileSystemWatchers.Clear();

            if (ev.Paths == null)
                return;

            foreach (var path in ev.Paths)
            {
                var watcher = new FileSystemWatcher(path);
                watcher.Created += WatcherOnCreated;
                watcher.Changed += WatcherOnChanged;
                watcher.Deleted += WatcherOnDeleted;
                watcher.Renamed += WatcherOnRenamed;
                watcher.EnableRaisingEvents = Enabled;

                fileSystemWatchers.Add(watcher);
            }

            if (InitialScan)
                RescanExistingFiles();
        }

        private void WatcherOnRenamed(object sender, RenamedEventArgs e)
        {
            EventBus.PublishEvent(new FileMonitorFileRenamed { FilePath = e.FullPath, OldFilePath = e.OldFullPath });
        }

        private void WatcherOnDeleted(object sender, FileSystemEventArgs e)
        {
            EventBus.PublishEvent(new FileMonitorFileDeleted { FilePath = e.FullPath });
        }

        private void WatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            EventBus.PublishEvent(new FileMonitorFileChanged { FilePath = e.FullPath });
        }

        private void WatcherOnCreated(object sender, FileSystemEventArgs e)
        {
            EventBus.PublishEvent(new FileMonitorFileCreated { FilePath = e.FullPath });
        }
    }
}
