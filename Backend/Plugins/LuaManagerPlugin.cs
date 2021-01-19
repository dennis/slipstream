using Slipstream.Backend.Services;
using Slipstream.Shared;
using Slipstream.Shared.Events.Lua;
using Slipstream.Shared.Factories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EventHandler = Slipstream.Shared.EventHandler;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    internal class LuaManagerPlugin : BasePlugin
    {
        private readonly IFileMonitorEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private readonly IPluginManager PluginManager;
        private readonly IPluginFactory PluginFactory;
        private readonly IEventSerdeService EventSerdeService;
        private readonly IDictionary<string, IPlugin> Scripts = new Dictionary<string, IPlugin>();

        // At bootup we will receive zero or more FileCreated events ending with a ScanCompleted.
        // If we count the (relevant) files found that launches LuaPlugin, we can keep an eye on PluginState
        // and determine when they are ready. Once all Lua scripts are ready, we can publish Initialized event
        private bool BootUp = true;

        private readonly List<string> WaitingForLuaScripts = new List<string>();

        private DateTime? BootupEventsDeadline;
        private readonly List<IEvent> CapturedBootupEvents = new List<IEvent>();

        public LuaManagerPlugin(string id, IFileMonitorEventFactory eventFactory, IEventBus eventBus, IPluginManager pluginManager, IPluginFactory pluginFactory, IEventSerdeService eventSerdeService) : base(id, "LuaManagerPlugin", "LuaManagerPlugin", "Core")
        {
            EventFactory = eventFactory;
            EventBus = eventBus;
            PluginManager = pluginManager;
            PluginFactory = pluginFactory;
            EventSerdeService = eventSerdeService;

            var fileMonitor = EventHandler.Get<Shared.EventHandlers.FileMonitor>();
            var @internal = EventHandler.Get<Shared.EventHandlers.Internal>();
            var lua = EventHandler.Get<Shared.EventHandlers.Lua>();

            fileMonitor.OnFileMonitorFileCreated += EventHandler_OnFileMonitorFileCreated;
            fileMonitor.OnFileMonitorFileDeleted += EventHandler_OnFileMonitorFileDeleted;
            fileMonitor.OnFileMonitorFileChanged += EventHandler_OnFileMonitorFileChanged;
            fileMonitor.OnFileMonitorFileRenamed += EventHandler_OnFileMonitorFileRenamed;
            fileMonitor.OnFileMonitorScanCompleted += EventHandler_OnFileMonitorScanCompleted;
            @internal.OnInternalPluginState += EventHandler_OnInternalPluginState;
            lua.OnLuaCommandDeduplicateEvents += (s, e) => EventHandler_OnLuaCommandDeduplicateEvents(e.Event);

            BootupEventsDeadline = DateTime.Now.AddMilliseconds(500);

            EventBus.PublishEvent(EventFactory.CreateFileMonitorCommandScan());
        }

        private void EventHandler_OnLuaCommandDeduplicateEvents(LuaCommandDeduplicateEvents @event)
        {
            foreach (var e in EventSerdeService.DeserializeMultiple(@event.Events))
            {
                CapturedBootupEvents.Add(e);
            }

            // Postponing deadline
            BootupEventsDeadline = DateTime.Now.AddMilliseconds(500);
        }

        private void EventHandler_OnInternalPluginState(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Internal.InternalPluginState> e)
        {
            if (e.Event.PluginStatus == "Registered" && e.Event.PluginName == "LuaPlugin")
            {
                WaitingForLuaScripts.Remove(e.Event.Id);

                if (WaitingForLuaScripts.Count == 0)
                {
                    // We're done
                    EventHandler.Get<Shared.EventHandlers.Internal>().OnInternalPluginState -= EventHandler_OnInternalPluginState;
                }
            }
        }

        private void EventHandler_OnFileMonitorScanCompleted(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.FileMonitor.FileMonitorScanCompleted> e)
        {
            BootUp = false;
        }

        private class LuaConfiguration : ILuaConfiguration
        {
            public string FilePath { get; set; } = "";
        }

        private void NewFile(string filePath)
        {
            if (Scripts.ContainsKey(filePath))
            {
                return;
            }

            string pluginId = Path.GetFileName(filePath);

            if (BootUp)
            {
                WaitingForLuaScripts.Add(pluginId);
            }

            var plugin = PluginFactory.CreatePlugin(pluginId, "LuaPlugin", (ILuaConfiguration)new LuaConfiguration { FilePath = filePath });

            PluginManager.RegisterPlugin(plugin);

            Scripts.Add(filePath, plugin);
        }

        private void DeletedFile(string filePath)
        {
            PluginManager.UnregisterPlugin(Scripts[filePath]);

            Scripts.Remove(filePath);
        }

        private void EventHandler_OnFileMonitorFileRenamed(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.FileMonitor.FileMonitorFileRenamed> e)
        {
            // If we're been unregistered (while app is running) and re-registered,we need to ignore these events
            // until we're ready (received the FileMonitorScanCompleted). We need to revisit this later.
            if (e.Event.FilePath == null || e.Event.OldFilePath == null || BootUp)
                return;

            if (IsApplicable(e.Event.OldFilePath))
                DeletedFile(e.Event.OldFilePath);
            if (IsApplicable(e.Event.FilePath))
                NewFile(e.Event.FilePath);
        }

        private void EventHandler_OnFileMonitorFileChanged(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.FileMonitor.FileMonitorFileChanged> e)
        {
            if (e.Event.FilePath == null || !IsApplicable(e.Event.FilePath) || BootUp)
                return;

            DeletedFile(e.Event.FilePath);
            NewFile(e.Event.FilePath);
        }

        private void EventHandler_OnFileMonitorFileDeleted(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.FileMonitor.FileMonitorFileDeleted> e)
        {
            if (e.Event.FilePath == null || !IsApplicable(e.Event.FilePath) || BootUp)
                return;

            DeletedFile(e.Event.FilePath);
        }

        private void EventHandler_OnFileMonitorFileCreated(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.FileMonitor.FileMonitorFileCreated> e)
        {
            if (e.Event.FilePath == null || !IsApplicable(e.Event.FilePath))
                return;

            NewFile(e.Event.FilePath);
        }

        private bool IsApplicable(string FilePath)
        {
            return Path.GetExtension(FilePath) == ".lua";
        }

        public override void Loop()
        {
            if (BootupEventsDeadline != null && BootupEventsDeadline <= DateTime.Now)
            {
                // We have collected the events published when LuaScripts were booting. To avoid
                // publishing the same events multiple times, we remove duplicates and then publish it

                // As the events likely got different timestamps, we need to reset them, so we easily
                // can find the duplicates. Here we just the timestamp to zero. As the timestamp will
                // be overwritten by eventbus upon publishing the event, we don't really change anything

                foreach (var e in CapturedBootupEvents.Distinct())
                {
                    e.Uptime = 0;
                }

                foreach (var e in CapturedBootupEvents.Distinct())
                {
                    EventBus.PublishEvent(e);
                }

                CapturedBootupEvents.Clear();

                BootupEventsDeadline = null;
            }
        }
    }
}