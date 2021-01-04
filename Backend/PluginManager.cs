#nullable enable

using Slipstream.Backend.Plugins;
using Slipstream.Backend.Services;
using Slipstream.Shared;
using System;
using System.Collections.Generic;
using static Slipstream.Shared.IEventFactory;

namespace Slipstream.Backend
{
    public class PluginManager : IPluginManager, IPluginFactory
    {
        private readonly IEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private readonly IDictionary<string, IPlugin> Plugins = new Dictionary<string, IPlugin>();
        private readonly IDictionary<string, PluginWorker> PluginWorkers = new Dictionary<string, PluginWorker>();
        private readonly IApplicationConfiguration ApplicationConfiguration;
        private readonly IStateService StateService;
        private readonly ITxrxService TxrxService;

        public PluginManager(IEventFactory eventFactory, IEventBus eventBus, IApplicationConfiguration applicationConfiguration, IStateService stateService, ITxrxService txrxService)
        {
            EventFactory = eventFactory;
            EventBus = eventBus;
            ApplicationConfiguration = applicationConfiguration;
            StateService = stateService;
            TxrxService = txrxService;
        }

        private void UnregisterPluginsWithoutLock()
        {
            foreach (var p in Plugins)
            {
                UnregisterPlugin(p.Value);
            }
            Plugins.Clear();
        }

        public void UnregisterPlugin(IPlugin p)
        {
            lock(Plugins)
            {
                UnregisterPluginWithoutLock(p);
            }
        }

        private void UnregisterPluginWithoutLock(IPlugin p)
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
                RegisterPluginWithoutLock(plugin);
            }
        }

        private void RegisterPluginWithoutLock(IPlugin plugin)
        {
            if(Plugins.ContainsKey(plugin.Id))
            {
                Plugins.Remove(plugin.Id);
            }

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
                EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole($"Can't find plugin '{pluginId}'"));
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
                UnregisterPluginWithoutLock(id);
            }
        }

        private void UnregisterPluginWithoutLock(string id)
        {
            if (Plugins.ContainsKey(id))
            {
                UnregisterPlugin(Plugins[id]);
                Plugins.Remove(id);
            }
        }

        public void RestartReconfigurablePlugins()
        {
            lock(Plugins)
            {
                var restartList = new List<IPlugin>();

                foreach(var oldPlugin in Plugins)
                {
                    if(oldPlugin.Value.Reconfigurable)
                    {
                        restartList.Add(oldPlugin.Value);
                    }
                }

                foreach(var plugin in restartList)
                {
                    UnregisterPluginWithoutLock(plugin);
                    Plugins.Remove(plugin.Id);

                    IPlugin newPlugin = CreatePlugin(plugin.Id, plugin.Name);

                    RegisterPluginWithoutLock(newPlugin);

                    if (plugin.Enabled)
                        EnablePlugin(newPlugin);
                }
            }

            EventBus.PublishEvent(EventFactory.CreateInternalInitialized());
        }

        public IPlugin CreatePlugin(string id, string name)
        {
            return name switch
            {
                "FileMonitorPlugin" => new FileMonitorPlugin(id, EventFactory, EventBus, ApplicationConfiguration),
                "FileTriggerPlugin" => new FileTriggerPlugin(id, EventFactory, EventBus, StateService, this),
                "AudioPlugin" => new AudioPlugin(id, EventFactory, EventBus, ApplicationConfiguration),
                "IRacingPlugin" => new IRacingPlugin(id, EventFactory, EventBus),
                "TwitchPlugin" => new TwitchPlugin(id, EventFactory, EventBus, ApplicationConfiguration),
                "TransmitterPlugin" => new TransmitterPlugin(id, EventFactory, EventBus, TxrxService, ApplicationConfiguration),
                "ReceiverPlugin" => new ReceiverPlugin(id, EventFactory, EventBus, TxrxService, ApplicationConfiguration),
                _ => throw new Exception($"Unknown plugin '{name}'"),
            };
        }

        public void Dispose()
        {
            lock(Plugins)
            {
                DisablePlugins();
                UnregisterPluginsWithoutLock();
                Plugins.Clear();
                foreach (var worker in PluginWorkers)
                {
                    worker.Value.Dispose();
                }
            }
        }

    }
}