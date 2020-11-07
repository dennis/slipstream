using Slipstream.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using EventHandler = Slipstream.Shared.EventHandler;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    class FileTriggerPlugin : Worker, IPlugin, IEventListener
    {
        public System.Guid Id { get; set; }
        public string Name => "FileTriggerPlugin";
        public string DisplayName => Name;

        public bool Enabled { get; internal set; }
        private IEventBusSubscription? EventBusSubscription;
        private readonly IEventBus EventBus;
        private readonly IDictionary<string, Guid> Scripts = new Dictionary<string, Guid>();

        public FileTriggerPlugin(IEventBus eventBus)
        {
            this.Id = Guid.NewGuid();
            this.EventBus = eventBus;
        }

        public void Disable(IEngine engine)
        {
            Enabled = false;
        }

        public void Enable(IEngine engine)
        {
            Enabled = true;
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
            Shared.EventHandler eventHandler = new Shared.EventHandler();

            eventHandler.OnInternalFileMonitorFileCreated += EventHandler_OnInternalFileMonitorFileCreated;
            eventHandler.OnInternalFileMonitorFileDeleted += EventHandler_OnInternalFileMonitorFileDeleted;
            eventHandler.OnInternalFileMonitorFileChanged += EventHandler_OnInternalFileMonitorFileChanged;
            eventHandler.OnInternalFileMonitorFileRenamed += EventHandler_OnInternalFileMonitorFileRenamed;

            while (!Stopped)
            {
                var e = EventBusSubscription?.NextEvent(250);

                if (Enabled)
                {
                    eventHandler.HandleEvent(e);
                }
            }
        }

        private void NewFile(string filePath)
        {
            Debug.WriteLine($"New file seen! {filePath}");

            string pluginName = "LuaPlugin";

            var ev = new Shared.Events.Internal.PluginRegister() { PluginName = pluginName, Enabled = true, Settings = new Slipstream.Shared.Events.Internal.LuaSettings() { FilePath = filePath } };

            EventBus.PublishEvent(ev);

            Scripts.Add(filePath, ev.Id);
        }

        private void DeletedFile(string filePath)
        {
            Debug.WriteLine($"Deleted file seen! {filePath}");

            var ev = new Shared.Events.Internal.PluginUnregister() { Id = Scripts[filePath] };

            EventBus.PublishEvent(ev);

            Scripts.Remove(filePath);
        }

        private void EventHandler_OnInternalFileMonitorFileRenamed(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Internal.FileMonitorFileRenamed> e)
        {
            if (e.Event.FilePath == null || e.Event.OldFilePath == null)
                return;

            if (IsApplicable(e.Event.OldFilePath))
                DeletedFile(e.Event.OldFilePath);
            if (IsApplicable(e.Event.FilePath))
                NewFile(e.Event.FilePath);
        }

        private void EventHandler_OnInternalFileMonitorFileChanged(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Internal.FileMonitorFileChanged> e)
        {
            if (e.Event.FilePath == null || !IsApplicable(e.Event.FilePath))
                return;

            DeletedFile(e.Event.FilePath);
            NewFile(e.Event.FilePath);
        }

        private void EventHandler_OnInternalFileMonitorFileDeleted(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Internal.FileMonitorFileDeleted> e)
        {
            if (e.Event.FilePath == null || !IsApplicable(e.Event.FilePath))
                return;

            DeletedFile(e.Event.FilePath);
        }

        private void EventHandler_OnInternalFileMonitorFileCreated(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Internal.FileMonitorFileCreated> e)
        {
            if (e.Event.FilePath == null || !IsApplicable(e.Event.FilePath))
                return;

            NewFile(e.Event.FilePath);
        }

        private bool IsApplicable(string FilePath)
        {
            return Path.GetExtension(FilePath) == ".lua";
        }
    }
}
