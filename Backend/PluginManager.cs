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

        // No events are sent while we are in Warmup. Once engine performs 
        // WarmupDone(), these will be handled
        private bool Warmup = true; 
        private readonly IList<IEvent> PendingEvents = new List<IEvent>();

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
            EmitEvent(new Shared.Events.Internal.PluginStateChanged() { Id = plugin.Id, PluginName = plugin.Name, PluginStatus = status, DisplayName = plugin.DisplayName });
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

        public void WarmupDone()
        {
            Debug.Assert(Warmup);

            Warmup = false;

            foreach (var e in PendingEvents)
                EmitEvent(e);

            PendingEvents.Clear();
        }

        private void EmitEvent(IEvent e)
        {
            if (Warmup)
                PendingEvents.Add(e);
            else
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