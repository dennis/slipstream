#nullable enable

using Slipstream.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Slipstream.Backend
{
    class PluginManager
    {
        private readonly IEngine Engine;
        private readonly IEventBus EventBus;
        private readonly IDictionary<Guid, IPlugin> Plugins = new Dictionary<Guid, IPlugin>();
        private readonly IList<IPlugin> PendingPluginsForEnable = new List<IPlugin>();
        private bool DisablePendingEnable;

        public PluginManager(IEngine engine, IEventBus eventBus)
        {
            Engine = engine;
            EventBus = eventBus;
        }

        public void UnregisterPlugins()
        {
            foreach (var p in Plugins)
            {
                UnregisterPlugin(p.Value);
            }
        }

        private void UnregisterPlugin(IPlugin p)
        {
            p.UnregisterPlugin(Engine);
            EmitePluginStateChanged(p, Shared.Events.Internal.PluginStatus.Unregistered);
        }

        private void EmitePluginStateChanged(IPlugin plugin, Shared.Events.Internal.PluginStatus status)
        {
            EventBus.PublishEvent(new Shared.Events.Internal.PluginStateChanged() { Id = plugin.Id, PluginName = plugin.Name, PluginStatus = status, DisplayName = plugin.DisplayName });
        }

        public void RegisterPlugin(IPlugin p)
        {
            p.RegisterPlugin(Engine);
            EmitePluginStateChanged(p, Shared.Events.Internal.PluginStatus.Registered);
        }

        public void EnablePlugin(IPlugin p)
        {
            p.Enable(Engine);
            EmitePluginStateChanged(p, Shared.Events.Internal.PluginStatus.Enabled);
        }

        public void DisablePlugins()
        {
            foreach (var p in Plugins)
            {
                DisablePlugin(p.Value);
            }
        }

        public void EnablePendingPlugins()
        {
            foreach (var plugin in PendingPluginsForEnable)
                EnablePlugin(plugin);

            PendingPluginsForEnable.Clear();

            DisablePendingEnable = true;
        }

        public void DisablePlugin(IPlugin p)
        {
            p.Disable(Engine);
            EmitePluginStateChanged(p, Shared.Events.Internal.PluginStatus.Disabled);
        }

        public void InitializePlugin(IPlugin plugin, bool enabled)
        {

            Plugins.Add(plugin.Id, plugin);
            RegisterPlugin(plugin);
            if (enabled)
            {
                if (DisablePendingEnable)
                    EnablePlugin(plugin);
                else
                    PendingPluginsForEnable.Add(plugin);
            }
        }

        public void FindPluginAndExecute(Guid pluginId, Action<IPlugin> a)
        {
            if (Plugins.TryGetValue(pluginId, out IPlugin plugin))
            {
                a(plugin);
            }
            else
            {
                Debug.WriteLine($"Plugin not found '{pluginId}'");
            }
        }

        public void UnregisterPlugin(Guid id)
        {
            UnregisterPlugin(Plugins[id]);
        }
    }
}