using Slipstream.Shared;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using EventHandler = Slipstream.Shared.EventHandler;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    class FileTriggerPlugin : BasePlugin
    {
        private readonly IEventBus EventBus;
        private readonly IDictionary<string, string> Scripts = new Dictionary<string, string>();

        // At bootup we will receive zero or more FileCreated events ending with a ScanCompleted. 
        // If we count the (relevant) files found that launches LuaPlugin, we can keep an eye on PluginState
        // and determine when they are ready. Once all Lua scripts are ready, we can publish Initialized event
        private bool BootUp = true;
        private readonly List<string> WaitingForLuaScripts = new List<string>();

        public FileTriggerPlugin(string id, IEventBus eventBus) : base(id, "FileTriggerPlugin", "FileTriggerPlugin", "Core")
        {
            this.EventBus = eventBus;

            EventHandler.OnFileMonitorFileCreated += EventHandler_OnFileMonitorFileCreated;
            EventHandler.OnFileMonitorFileDeleted += EventHandler_OnFileMonitorFileDeleted;
            EventHandler.OnFileMonitorFileChanged += EventHandler_OnFileMonitorFileChanged;
            EventHandler.OnFileMonitorFileRenamed += EventHandler_OnFileMonitorFileRenamed;
            EventHandler.OnFileMonitorScanCompleted += EventHandler_OnFileMonitorScanCompleted;
            EventHandler.OnInternalPluginState += EventHandler_OnInternalPluginState;
        }

        private void EventHandler_OnInternalPluginState(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.Internal.PluginState> e)
        {
            if(e.Event.PluginStatus == "Registered" && e.Event.PluginName == "LuaPlugin")
            {
                WaitingForLuaScripts.Remove(e.Event.Id);

                if(WaitingForLuaScripts.Count == 0)
                {
                    // We're done
                    EventHandler.OnInternalPluginState -= EventHandler_OnInternalPluginState;

                    EventBus.PublishEvent(new Shared.Events.Internal.Initialized());
                }
            }
        }

        private void EventHandler_OnFileMonitorScanCompleted(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.FileMonitor.FileMonitorScanCompleted> e)
        {
            Debug.Assert(BootUp);

            BootUp = false;
        }

        private void NewFile(string filePath)
        {
            if (Scripts.ContainsKey(filePath))
            {
                return;
            }

            string pluginName = "LuaPlugin";
            string pluginId = Path.GetFileName(filePath);

            if (BootUp)
            {
                WaitingForLuaScripts.Add(pluginId);
            }


            EventBus.PublishEvent(new Shared.Events.Internal.CommandPluginRegister() { Id = pluginId, PluginName = pluginName, Settings = GetLuaSettings(pluginId, filePath) });
            EventBus.PublishEvent(new Shared.Events.Internal.CommandPluginEnable() { Id = pluginId });

            Scripts.Add(filePath, pluginId);
        }

        private IEvent GetLuaSettings(string pluginId, string filePath)
        {
            return new Slipstream.Shared.Events.Setting.LuaSettings() { PluginId = pluginId, FilePath = filePath };
        }

        private void DeletedFile(string filePath)
        {
            var ev = new Shared.Events.Internal.CommandPluginUnregister() { Id = Scripts[filePath] };

            EventBus.PublishEvent(ev);

            Scripts.Remove(filePath);
        }

        private void EventHandler_OnFileMonitorFileRenamed(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.FileMonitor.FileMonitorFileRenamed> e)
        {
            if (e.Event.FilePath == null || e.Event.OldFilePath == null)
                return;

            Debug.Assert(!BootUp);

            if (IsApplicable(e.Event.OldFilePath))
                DeletedFile(e.Event.OldFilePath);
            if (IsApplicable(e.Event.FilePath))
                NewFile(e.Event.FilePath);
        }

        private void EventHandler_OnFileMonitorFileChanged(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.FileMonitor.FileMonitorFileChanged> e)
        {
            if (e.Event.FilePath == null || !IsApplicable(e.Event.FilePath))
                return;

            Debug.Assert(!BootUp);

            DeletedFile(e.Event.FilePath);
            NewFile(e.Event.FilePath);
        }

        private void EventHandler_OnFileMonitorFileDeleted(EventHandler source, EventHandler.EventHandlerArgs<Shared.Events.FileMonitor.FileMonitorFileDeleted> e)
        {
            if (e.Event.FilePath == null || !IsApplicable(e.Event.FilePath))
                return;

            Debug.Assert(!BootUp);

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
