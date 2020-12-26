#nullable enable

using Slipstream.Shared;
using System;
using System.Collections.Generic;

namespace Slipstream.Backend
{
    class PluginManager
    {
        private readonly IEngine Engine;
        private readonly IEventBus EventBus;
        private readonly IDictionary<string, IPlugin> Plugins = new Dictionary<string, IPlugin>();
        private readonly IDictionary<string, PluginWorker> PluginWorkers = new Dictionary<string, PluginWorker>();

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
            PluginWorkers[p.WorkerName].RemovePlugin(p);
            EmitPluginStateChanged(p, Shared.Events.Internal.PluginStatus.Unregistered);
        }

        private void EmitPluginStateChanged(IPlugin plugin, Shared.Events.Internal.PluginStatus status)
        {
            EmitEvent(new Shared.Events.Internal.PluginState() { Id = plugin.Id, PluginName = plugin.Name, PluginStatus = status, DisplayName = plugin.DisplayName });
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
                    worker = new PluginWorker(plugin.WorkerName, Engine.RegisterListener(), EventBus);
                    worker.Start();
                    PluginWorkers.Add(worker.Name, worker);
                }

                worker.AddPlugin(plugin);

                Plugins.Add(plugin.Id, plugin);

                EmitPluginStateChanged(plugin, Shared.Events.Internal.PluginStatus.Registered);
            }
        }

        public void EnablePlugin(IPlugin p)
        {
            p.Enable();
        }

        public void DisablePlugins()
        {
            foreach (var p in Plugins)
            {
                DisablePlugin(p.Value);
            }
        }

        private void EmitEvent(IEvent e)
        {
            EventBus.PublishEvent(e);
        }

        public void DisablePlugin(IPlugin p)
        {
            p.Disable();
        }

        public void FindPluginAndExecute(string pluginId, Action<IPlugin> a)
        {
            if (Plugins.TryGetValue(pluginId, out IPlugin plugin))
            {
                a(plugin);
            }
            else
            {
                throw new Exception($"Can't find plugin '{pluginId}'");
            }
        }

        internal void ForAllPluginsExecute(Action<IPlugin> a)
        {
            lock(Plugins)
            {
                foreach(var p in Plugins)
                {
                    a(p.Value);
                }
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