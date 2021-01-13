#nullable enable

using Serilog;
using Slipstream.Backend.Plugins;
using Slipstream.Backend.Services;
using Slipstream.Shared;
using Slipstream.Shared.Factories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Slipstream.Shared.Factories.IInternalEventFactory;

namespace Slipstream.Backend
{
    public class PluginManager : IPluginManager, IPluginFactory
    {
        private readonly IEventFactory EventFactory;
        private readonly IInternalEventFactory InternalEventFactory;
        private readonly IFileMonitorEventFactory FileMonitorEventFactory;
        private readonly IIRacingEventFactory IRacingEventFactory;
        private readonly ITwitchEventFactory TwitchEventFactory;

        private readonly IEventBus EventBus;
        private readonly IDictionary<string, IPlugin> Plugins = new Dictionary<string, IPlugin>();
        private readonly IDictionary<string, PluginWorker> PluginWorkers = new Dictionary<string, PluginWorker>();
        private readonly IApplicationConfiguration ApplicationConfiguration;
        private readonly IStateService StateService;
        private readonly ITxrxService TxrxService;
        private readonly IEventSerdeService EventSerdeService;
        private readonly ILogger Logger;

        public PluginManager(
            IEventFactory eventFactory,
            IEventBus eventBus,
            IApplicationConfiguration applicationConfiguration,
            IStateService stateService,
            ITxrxService txrxService,
            IEventSerdeService eventSerdeService,
            ILogger logger
        )
        {
            EventFactory = eventFactory;
            InternalEventFactory = eventFactory.Get<IInternalEventFactory>();
            FileMonitorEventFactory = eventFactory.Get<IFileMonitorEventFactory>();
            IRacingEventFactory = eventFactory.Get<IIRacingEventFactory>();
            TwitchEventFactory = eventFactory.Get<ITwitchEventFactory>();
            EventBus = eventBus;
            ApplicationConfiguration = applicationConfiguration;
            StateService = stateService;
            TxrxService = txrxService;
            EventSerdeService = eventSerdeService;
            Logger = logger;
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
            p.Dispose();
            Logger.Verbose("Removed plugin: {pluginId}", p.Id);
        }

        private void EmitPluginStateChanged(IPlugin plugin, PluginStatusEnum pluginStatus)
        {
            EmitEvent(InternalEventFactory.CreateInternalPluginState(plugin.Id, plugin.Name, plugin.DisplayName, pluginStatus));
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
                Logger.Verbose("Assigning plugin {PluginId} to worker {WorkerName}", plugin.Id, plugin.WorkerName);

                worker = PluginWorkers[plugin.WorkerName];
            }
            else
            {
                Logger.Verbose("Creating worker {workerName} for {PluginId}", plugin.WorkerName, plugin.Id);
                worker = new PluginWorker(plugin.WorkerName, EventBus.RegisterListener(), InternalEventFactory, EventBus);
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
                Logger.Error("Can't find plugin {pluginId}", plugin);
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
                "FileMonitorPlugin" => new FileMonitorPlugin(id, FileMonitorEventFactory, eventBus, ApplicationConfiguration),
                "LuaManagerPlugin" => new LuaManagerPlugin(id, FileMonitorEventFactory, eventBus, this, this, EventSerdeService),
                "AudioPlugin" => new AudioPlugin(id, Logger.ForContext(typeof(AudioPlugin)), ApplicationConfiguration),
                "IRacingPlugin" => new IRacingPlugin(id, IRacingEventFactory, eventBus),
                "TwitchPlugin" => new TwitchPlugin(id, Logger.ForContext(typeof(TwitchPlugin)), TwitchEventFactory, eventBus, ApplicationConfiguration),
                "TransmitterPlugin" => new TransmitterPlugin(id, Logger.ForContext(typeof(TransmitterPlugin)), InternalEventFactory, eventBus, TxrxService, ApplicationConfiguration),
                "ReceiverPlugin" => new ReceiverPlugin(id, Logger.ForContext(typeof(ReceiverPlugin)), InternalEventFactory, eventBus, TxrxService, ApplicationConfiguration),
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
                "LuaPlugin" when configuration is ILuaConfiguration => new LuaPlugin(
                    pluginId,
                    Logger.ForContext(typeof(LuaPlugin)),
                    EventFactory,
                    EventBus,
                    StateService,
                    EventSerdeService,
                    configuration as ILuaConfiguration
                ),
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