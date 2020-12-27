using Slipstream.Shared;
using Slipstream.Shared.Events.Internal;
using System.Collections.Generic;
using System.IO;
using EventHandler = Slipstream.Shared.EventHandler;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    class FileTriggerPlugin : BasePlugin
    {
        private readonly IEventBus EventBus;
        private readonly IDictionary<string, string> Scripts = new Dictionary<string, string>();

        public FileTriggerPlugin(string id, IEventBus eventBus) : base(id, "FileTriggerPlugin", "FileTriggerPlugin", "Core")
        {
            this.EventBus = eventBus;

            EventHandler.OnInternalFileMonitorFileCreated += EventHandler_OnInternalFileMonitorFileCreated;
            EventHandler.OnInternalFileMonitorFileDeleted += EventHandler_OnInternalFileMonitorFileDeleted;
            EventHandler.OnInternalFileMonitorFileChanged += EventHandler_OnInternalFileMonitorFileChanged;
            EventHandler.OnInternalFileMonitorFileRenamed += EventHandler_OnInternalFileMonitorFileRenamed;
        }

        private void NewFile(string filePath)
        {
            if (Scripts.ContainsKey(filePath))
            {
                return;
            }

            string pluginName = "LuaPlugin";
            string pluginId = Path.GetFileName(filePath);

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
