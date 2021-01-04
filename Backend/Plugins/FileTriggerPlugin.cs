﻿using Slipstream.Backend.Services;
using Slipstream.Shared;
using System.Collections.Generic;
//using System.Diagnostics;
using System.IO;
using EventHandler = Slipstream.Shared.EventHandler;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    class FileTriggerPlugin : BasePlugin
    {
        private readonly IEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private readonly IStateService StateService;
        private readonly IPluginManager PluginManager;
        private readonly IDictionary<string, IPlugin> Scripts = new Dictionary<string, IPlugin>();

        // At bootup we will receive zero or more FileCreated events ending with a ScanCompleted. 
        // If we count the (relevant) files found that launches LuaPlugin, we can keep an eye on PluginState
        // and determine when they are ready. Once all Lua scripts are ready, we can publish Initialized event
        private bool BootUp = true;
        private readonly List<string> WaitingForLuaScripts = new List<string>();

        public FileTriggerPlugin(string id, IEventFactory eventFactory, IEventBus eventBus, IStateService stateService, IPluginManager pluginManager) : base(id, "FileTriggerPlugin", "FileTriggerPlugin", "Core")
        {
            EventFactory = eventFactory;
            EventBus = eventBus;
            StateService = stateService;
            PluginManager = pluginManager;

            EventHandler.OnFileMonitorFileCreated += EventHandler_OnFileMonitorFileCreated;
            EventHandler.OnFileMonitorFileDeleted += EventHandler_OnFileMonitorFileDeleted;
            EventHandler.OnFileMonitorFileChanged += EventHandler_OnFileMonitorFileChanged;
            EventHandler.OnFileMonitorFileRenamed += EventHandler_OnFileMonitorFileRenamed;
            EventHandler.OnFileMonitorScanCompleted += EventHandler_OnFileMonitorScanCompleted;
            EventHandler.OnInternalPluginState += EventHandler_OnInternalPluginState;
        }

        private void EventHandler_OnInternalPluginState(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Internal.InternalPluginState> e)
        {
            if(e.Event.PluginStatus == "Registered" && e.Event.PluginName == "LuaPlugin")
            {
                WaitingForLuaScripts.Remove(e.Event.Id);

                if(WaitingForLuaScripts.Count == 0)
                {
                    // We're done
                    EventHandler.OnInternalPluginState -= EventHandler_OnInternalPluginState;

                    EventBus.PublishEvent(EventFactory.CreateInternalInitialized());
                }
            }
        }

        private void EventHandler_OnFileMonitorScanCompleted(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.FileMonitor.FileMonitorScanCompleted> e)
        {
            BootUp = false;
        }

        class LuaConfiguration : ILuaConfiguration
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

            // Use PluginManager director
            var plugin = new LuaPlugin(pluginId, EventFactory, EventBus, StateService, new LuaConfiguration { FilePath = filePath });
            PluginManager.RegisterPlugin(plugin);
            PluginManager.EnablePlugin(plugin);

            Scripts.Add(filePath, plugin);
        }

        private void DeletedFile(string filePath)
        {
            PluginManager.UnregisterPlugin(Scripts[filePath]);

            Scripts.Remove(filePath);
        }

        private void EventHandler_OnFileMonitorFileRenamed(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.FileMonitor.FileMonitorFileRenamed> e)
        {
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
    }
}
