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
        private readonly IDictionary<string, IPlugin> Plugins = new Dictionary<string, IPlugin>();
        private readonly IDictionary<string, PluginWorker> PluginWorkers = new Dictionary<string, PluginWorker>();
        private readonly IList<IPlugin> PendingPluginsForEnable = new List<IPlugin>();
        private bool DisablePendingEnable;

        public PluginManager(IEngine engine, IEventBus eventBus)
        {
            Engine = engine;
            EventBus = eventBus;
        }

        public void UnregisterPlugins()
        {
            lock (Plugins)
            {
                foreach (var p in Plugins)
                {
                    UnregisterPlugin(p.Value);
                }
                Plugins.Clear();
            }
        }

        private void UnregisterPlugin(IPlugin p)
        {
            p.UnregisterPlugin(Engine);
            PluginWorkers[p.WorkerName].RemovePlugin(p);
            EmitPluginStateChanged(p, Shared.Events.Internal.PluginStatus.Unregistered);
        }

        private void EmitPluginStateChanged(IPlugin plugin, Shared.Events.Internal.PluginStatus status)
        {
            EventBus.PublishEvent(new Shared.Events.Internal.PluginStateChanged() { Id = plugin.Id, PluginName = plugin.Name, PluginStatus = status, DisplayName = plugin.DisplayName });
        }

        public void RegisterPlugin(IPlugin plugin)
        {
            lock (Plugins)
            {
                PluginWorker? worker;

                if (PluginWorkers.ContainsKey(plugin.WorkerName))
                {
                    worker = PluginWorkers[plugin.WorkerName];
                }
                else
                {
                    worker = new PluginWorker(plugin.WorkerName);
                    worker.Start();
                    PluginWorkers.Add(worker.Name, worker);
                }

                worker.AddPlugin(plugin);

                plugin.RegisterPlugin(Engine);
                Plugins.Add(plugin.Id, plugin);

                EmitPluginStateChanged(plugin, Shared.Events.Internal.PluginStatus.Registered);
            }
        }

        public void EnablePlugin(IPlugin p)
        {
            p.Enable(Engine);
            EmitPluginStateChanged(p, Shared.Events.Internal.PluginStatus.Enabled);
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
            EmitPluginStateChanged(p, Shared.Events.Internal.PluginStatus.Disabled);
        }

        public void InitializePlugin(IPlugin plugin, bool enabled)
        {
            Debug.WriteLine($"Plugin {plugin.Name} wants WorkerName {plugin.WorkerName}");

            RegisterPlugin(plugin);

            if (enabled)
            {
                if (DisablePendingEnable)
                    EnablePlugin(plugin);
                else
                    PendingPluginsForEnable.Add(plugin);
            }
        }

        public void FindPluginAndExecute(string pluginId, Action<IPlugin> a)
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

        public void UnregisterPlugin(string id)
        {
            lock (Plugins)
            {
                if (Plugins.ContainsKey(id))
                {
                    UnregisterPlugin(Plugins[id]);
                    Plugins.Remove(id);
                }
            }
        }

        internal void Dispose()
        {
            DisablePlugins();
            UnregisterPlugins();
            Plugins.Clear();
            foreach (var worker in PluginWorkers)
            {
                worker.Value.Dispose();
            }
        }
    }
}