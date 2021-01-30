using Serilog;
using Slipstream.Backend;
using Slipstream.Backend.Plugins;
using Slipstream.Backend.Services;
using Slipstream.Components.FileMonitor;
using Slipstream.Components.FileMonitor.Events;
using Slipstream.Components.Internal.Events;
using Slipstream.Components.Lua.Events;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable enable

namespace Slipstream.Components.Lua.Plugins
{
    internal class LuaManagerPlugin : BasePlugin
    {
        private readonly ILogger Logger;
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

        public LuaManagerPlugin(
            IEventHandlerController eventHandlerController,
            string id,
            ILogger logger,
            IFileMonitorEventFactory eventFactory,
            IEventBus eventBus,
            IPluginManager pluginManager,
            IPluginFactory pluginFactory,
            IServiceLocator serviceLocator
        ) : base(eventHandlerController, id, "LuaManagerPlugin", id)
        {
            Logger = logger;
            EventFactory = eventFactory;
            EventBus = eventBus;
            PluginManager = pluginManager;
            PluginFactory = pluginFactory;
            EventSerdeService = serviceLocator.Get<IEventSerdeService>();

            var fileMonitor = EventHandlerController.Get<Components.FileMonitor.EventHandler.FileMonitor>();
            var @internal = EventHandlerController.Get<Components.Internal.EventHandler.Internal>();
            var lua = EventHandlerController.Get<EventHandler.Lua>();

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

        private void EventHandler_OnInternalPluginState(IEventHandlerController source, EventHandlerArgs<InternalPluginState> e)
        {
            if (e.Event.PluginStatus == "Registered" && e.Event.PluginName == "LuaPlugin")
            {
                WaitingForLuaScripts.Remove(e.Event.Id);

                if (WaitingForLuaScripts.Count == 0)
                {
                    // We're done
                    EventHandlerController.Get<Slipstream.Components.Internal.EventHandler.Internal>().OnInternalPluginState -= EventHandler_OnInternalPluginState;
                }
            }
        }

        private void EventHandler_OnFileMonitorScanCompleted(IEventHandlerController source, EventHandlerArgs<FileMonitorScanCompleted> e)
        {
            BootUp = false;
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

            try
            {
                var plugin = PluginFactory.CreatePlugin(pluginId, "LuaPlugin", new Parameters { { "filepath", filePath } });

                PluginManager.RegisterPlugin(plugin);

                Scripts.Add(filePath, plugin);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Failed creating plugin for {filePath}: {e.Message}");
            }
        }

        private void DeletedFile(string filePath)
        {
            PluginManager.UnregisterPlugin(Scripts[filePath]);

            Scripts.Remove(filePath);
        }

        private void EventHandler_OnFileMonitorFileRenamed(IEventHandlerController source, EventHandlerArgs<FileMonitorFileRenamed> e)
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

        private void EventHandler_OnFileMonitorFileChanged(IEventHandlerController source, EventHandlerArgs<FileMonitorFileChanged> e)
        {
            if (e.Event.FilePath == null || !IsApplicable(e.Event.FilePath) || BootUp)
                return;

            DeletedFile(e.Event.FilePath);
            NewFile(e.Event.FilePath);
        }

        private void EventHandler_OnFileMonitorFileDeleted(IEventHandlerController source, EventHandlerArgs<FileMonitorFileDeleted> e)
        {
            if (e.Event.FilePath == null || !IsApplicable(e.Event.FilePath) || BootUp)
                return;

            DeletedFile(e.Event.FilePath);
        }

        private void EventHandler_OnFileMonitorFileCreated(IEventHandlerController source, EventHandlerArgs<FileMonitorFileCreated> e)
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