using Slipstream.Shared;
using Slipstream.Shared.Events.Internal;
using Slipstream.Shared.Events.Setting;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    class FileMonitorPlugin : IPlugin
    {
        public string Id { get; }
        public string Name => "FileMonitorPlugin";
        public string DisplayName => Name;
        public bool Enabled { get; internal set; }
        public string WorkerName => "Core";
        public EventHandler EventHandler { get; } = new EventHandler();

        private readonly IEventBus EventBus;
        private readonly IList<FileSystemWatcher> fileSystemWatchers = new List<FileSystemWatcher>();

        public FileMonitorPlugin(string id, IEvent settings, IEventBus eventBus)
        {
            Id = id;
            this.EventBus = eventBus;

            if (settings is FileMonitorSettings typedSettings)
                OnFileMonitorSettings(typedSettings);
            else
                throw new System.Exception($"Unexpected event as Exception {settings}");

            EventHandler.OnInternalPluginsReady += EventHandler_OnInternalPluginsReady;
            EventHandler.OnSettingFileMonitorSettings += (s, e) => OnFileMonitorSettings(e.Event);
        }

        public void Disable(IEngine engine)
        {
            UpdateWatchers();
        }

        private void UpdateWatchers()
        {
            foreach (var watcher in fileSystemWatchers)
                watcher.EnableRaisingEvents = Enabled;
        }

        public void Enable(IEngine engine)
        {
            UpdateWatchers();
        }

        public void RegisterPlugin(IEngine engine)
        {
        }

        public void UnregisterPlugin(IEngine engine)
        {
        }

        public void Loop()
        {
        }

        private void EventHandler_OnInternalPluginsReady(EventHandler source, EventHandler.EventHandlerArgs<PluginsReady> e)
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
                Debug.WriteLine($"OnFileMonitorSettings: Adding watcher for {path}");

                var watcher = new FileSystemWatcher(path);
                watcher.Created += WatcherOnCreated;
                watcher.Changed += WatcherOnChanged;
                watcher.Deleted += WatcherOnDeleted;
                watcher.Renamed += WatcherOnRenamed;

                fileSystemWatchers.Add(watcher);
            }
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
