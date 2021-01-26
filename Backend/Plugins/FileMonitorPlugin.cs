using Slipstream.Shared;
using Slipstream.Shared.Events.FileMonitor;
using Slipstream.Shared.Factories;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    internal class FileMonitorPlugin : BasePlugin
    {
        private static readonly DictionaryValidator ConfigurationValidator;
        private readonly IFileMonitorEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private readonly IList<FileSystemWatcher> fileSystemWatchers = new List<FileSystemWatcher>();

        static FileMonitorPlugin()
        {
            ConfigurationValidator = new DictionaryValidator()
                .PermitArray("paths", (a) => a.RequireString());
        }

        public FileMonitorPlugin(IEventHandlerController eventHandlerController, string id, IFileMonitorEventFactory eventFactory, IEventBus eventBus, Parameters configuration) : base(eventHandlerController, id, "FileMonitorPlugin", id, "Core", true)
        {
            EventFactory = eventFactory;
            EventBus = eventBus;

            var FileMonitor = EventHandlerController.Get<Slipstream.Shared.EventHandlers.FileMonitor>();
            FileMonitor.OnFileMonitorCommandScan += (s, e) => ScanExistingFiles();

            ConfigurationValidator.Validate(configuration);

            string[] paths = new string[] { "Scripts" };

            if (configuration.ContainsKey("paths"))
            {
                paths = (configuration.Extract<Dictionary<dynamic, dynamic>>("paths").Values.Cast<string>()!).ToArray();
            }

            StartMonitoring(paths);

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