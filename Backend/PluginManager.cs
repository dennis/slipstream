﻿#nullable enable

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

        private void EmitEvent(IEvent e)
        {
            EventBus.PublishEvent(e);
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
                }
            }
        }

        public IPlugin CreatePlugin(string id, string name)
        {
            return CreatePlugin(id, name, EventBus);
        }

        public IPlugin CreatePlugin(string id, string name, IEventBus eventBus)
        {
            return name switch
            {
                "FileMonitorPlugin" => new FileMonitorPlugin(id, EventFactory, eventBus, ApplicationConfiguration),
                "FileTriggerPlugin" => new FileTriggerPlugin(id, EventFactory, eventBus, this, this),
                "AudioPlugin" => new AudioPlugin(id, EventFactory, eventBus, ApplicationConfiguration),
                "IRacingPlugin" => new IRacingPlugin(id, EventFactory, eventBus),
                "TwitchPlugin" => new TwitchPlugin(id, EventFactory, eventBus, ApplicationConfiguration),
                "TransmitterPlugin" => new TransmitterPlugin(id, EventFactory, eventBus, TxrxService, ApplicationConfiguration),
                "ReceiverPlugin" => new ReceiverPlugin(id, EventFactory, eventBus, TxrxService, ApplicationConfiguration),
                _ => throw new Exception($"Unknown plugin '{name}'"),
            };
        }

        public IPlugin CreatePlugin<T>(string pluginId, string name, T configuration)
        {
            return CreatePlugin(pluginId, name, EventBus, configuration);
        }

        public IPlugin CreatePlugin<T>(string pluginId, string name, IEventBus eventBus, T configuration)
        {
            return name switch
            {
#pragma warning disable CS8604 // Possible null reference argument.
                "LuaPlugin" when configuration is ILuaConfiguration => new LuaPlugin(pluginId, EventFactory, EventBus, StateService, configuration as ILuaConfiguration),
#pragma warning restore CS8604 // Possible null reference argument.
                _ => throw new Exception($"Unknown configurable plugin '{name}'"),
            };
        }

        public void Dispose()
        {
            lock(Plugins)
            {
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