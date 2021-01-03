#nullable enable

using Slipstream.Shared;
using System;
using System.Collections.Generic;
using static Slipstream.Shared.IEventFactory;

namespace Slipstream.Backend
{
    public interface IPluginManager : IDisposable
    {
        public void UnregisterPlugins();
        public void UnregisterPlugin(IPlugin p);
        public void UnregisterPlugin(string id);
        public void RegisterPlugin(IPlugin plugin);
        public void EnablePlugin(IPlugin p);
        public void DisablePlugins();
        public void DisablePlugin(IPlugin p);
        public void FindPluginAndExecute(string pluginId, Action<IPlugin> a);
        public void ForAllPluginsExecute(Action<IPlugin> a);
    }

    public class PluginManager : IPluginManager
    {
        private readonly IEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private readonly IDictionary<string, IPlugin> Plugins = new Dictionary<string, IPlugin>();
        private readonly IDictionary<string, PluginWorker> PluginWorkers = new Dictionary<string, PluginWorker>();

        public PluginManager(IEventFactory eventFactory, IEventBus eventBus)
        {
            EventFactory = eventFactory;
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

        public void UnregisterPlugin(IPlugin p)
        {
            PluginWorkers[p.WorkerName].RemovePlugin(p);
            EmitPluginStateChanged(p, PluginStatusEnum.Unregistered);
        }

        private void EmitPluginStateChanged(IPlugin plugin, PluginStatusEnum pluginStatus)
        {
            EmitEvent(EventFactory.CreateInternalPluginState(plugin.Id, plugin.Name, plugin.DisplayName, pluginStatus));
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
                    worker = new PluginWorker(plugin.WorkerName, EventBus.RegisterListener(), EventFactory, EventBus);
                    worker.Start();
                    PluginWorkers.Add(worker.Name, worker);
                }

                worker.AddPlugin(plugin);

                Plugins.Add(plugin.Id, plugin);

                EmitPluginStateChanged(plugin, PluginStatusEnum.Registered);
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

        public void ForAllPluginsExecute(Action<IPlugin> a)
        {
            lock (Plugins)
            {
                foreach (var p in Plugins)
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

        public void Dispose()
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