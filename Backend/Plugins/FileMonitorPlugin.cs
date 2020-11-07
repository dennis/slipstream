using Slipstream.Shared;
using Slipstream.Shared.Events.Internal;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    class FileMonitorPlugin : Worker, IPlugin, IEventListener
    {
        public System.Guid Id { get; set; }
        public string Name => "FileMonitorPlugin";
        public string DisplayName => Name;

        public bool Enabled { get; internal set; }
        private IEventBusSubscription? EventBusSubscription;
        private readonly IEventBus EventBus;
        private readonly IList<FileSystemWatcher> fileSystemWatchers = new List<FileSystemWatcher>();

        public FileMonitorPlugin(IEvent settings, IEventBus eventBus)
        {
            Id = System.Guid.NewGuid();
            this.EventBus = eventBus;

            if (settings is FileMonitorSettings typedSettings)
                OnFileMonitorSettings(typedSettings);
            else
                throw new System.Exception($"Unexpected event as Exception {settings}");
        }

        public void Disable(IEngine engine)
        {
            Enabled = false;

            UpdateWatchers();
        }

        private void UpdateWatchers()
        {
            foreach (var watcher in fileSystemWatchers)
                watcher.EnableRaisingEvents = Enabled;
        }

        public void Enable(IEngine engine)
        {
            Enabled = true;

            UpdateWatchers();
        }

        public void RegisterPlugin(IEngine engine)
        {
            EventBusSubscription = engine.RegisterListener();

            Start();
        }

        public void UnregisterPlugin(IEngine engine)
        {
            EventBusSubscription?.Dispose();
            EventBusSubscription = null;

            Stop();
        }

        protected override void Main()
        {
            EventHandler eventHandler = new EventHandler();
            eventHandler.OnInternalPluginsReady += EventHandler_OnInternalPluginsReady;
            eventHandler.OnInternalFileMonitorSettings += (s, e) => OnFileMonitorSettings(e.Event);

            while (!Stopped)
            {
                var e = EventBusSubscription?.NextEvent(250);

                if (Enabled)
                {
                    eventHandler.HandleEvent(e);
                }
            }
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
