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
        private readonly IDictionary<string, IPlugin> Plugins;

        public PluginManager(IEngine engine, IEventBus eventBus)
        {
            Engine = engine;
            EventBus = eventBus;
            Plugins = new Dictionary<string, IPlugin>();
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
            EventBus.PublishEvent(new Shared.Events.Internal.PluginStateChanged() { PluginName = plugin.Name, PluginStatus = status });
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

        public void DisablePlugin(IPlugin p)
        {
            p.Disable(Engine);
            EmitePluginStateChanged(p, Shared.Events.Internal.PluginStatus.Disabled);
        }

        public void InitializePlugin(IPlugin plugin, bool enabled)
        {
            Plugins.Add(plugin.Name, plugin);
            RegisterPlugin(plugin);
            if (enabled)
                EnablePlugin(plugin);
        }

        public void FindPluginAndExecute(string pluginName, Action<IPlugin> a)
        {
            if (Plugins.TryGetValue(pluginName, out IPlugin plugin))
            {
                a(plugin);
            }
            else
            {
                Debug.WriteLine($"Plugin not found '{pluginName}'");
            }
        }
    }
}