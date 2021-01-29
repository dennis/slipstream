#nullable enable

using Serilog;
using Slipstream.Backend.Plugins;
using Slipstream.Backend.Services;
using Slipstream.Shared;
using Slipstream.Shared.Factories;
using Slipstream.Shared.Helpers.StrongParameters;
using System;
using System.Collections.Generic;
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
        private readonly IAudioEventFactory AudioEventFactory;
        private readonly IServiceLocator ServiceLocator;

        private readonly IEventBus EventBus;
        private readonly IDictionary<string, IPlugin> Plugins = new Dictionary<string, IPlugin>();
        private readonly IDictionary<string, PluginWorker> PluginWorkers = new Dictionary<string, PluginWorker>();
        private readonly ILogger Logger;
        private readonly EventHandlerControllerBuilder EventHandlerControllerBuilder;

        public PluginManager(
            IEventFactory eventFactory,
            IEventBus eventBus,
            IServiceLocator serviceLocator,
            ILogger logger,
            EventHandlerControllerBuilder eventHandlerControllerBuilder)
        {
            EventFactory = eventFactory;
            InternalEventFactory = eventFactory.Get<IInternalEventFactory>();
            FileMonitorEventFactory = eventFactory.Get<IFileMonitorEventFactory>();
            IRacingEventFactory = eventFactory.Get<IIRacingEventFactory>();
            TwitchEventFactory = eventFactory.Get<ITwitchEventFactory>();
            AudioEventFactory = eventFactory.Get<IAudioEventFactory>();
            EventBus = eventBus;
            ServiceLocator = serviceLocator;
            Logger = logger;
            EventHandlerControllerBuilder = eventHandlerControllerBuilder;
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
            lock (Plugins)
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
            if (Plugins.ContainsKey(plugin.Id))
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

        public IPlugin CreatePlugin(string pluginId, string name, Parameters configuration)
        {
            return CreatePlugin(pluginId, name, EventBus, configuration);
        }

        public IPlugin CreatePlugin(string pluginId, string name, IEventBus eventBus, Parameters configuration)
        {
            return name switch
            {
                "LuaPlugin" => new LuaPlugin(
                    EventHandlerControllerBuilder.CreateEventHandlerController(),
                    pluginId,
                    Logger.ForContext(typeof(LuaPlugin)),
                    EventFactory,
                    eventBus,
                    ServiceLocator,
                    configuration
                ),
                "FileMonitorPlugin" => new FileMonitorPlugin(EventHandlerControllerBuilder.CreateEventHandlerController(), pluginId, FileMonitorEventFactory, eventBus, configuration),
                "LuaManagerPlugin" => new LuaManagerPlugin(EventHandlerControllerBuilder.CreateEventHandlerController(), pluginId, Logger.ForContext(typeof(LuaPlugin)), FileMonitorEventFactory, eventBus, this, this, ServiceLocator),
                "AudioPlugin" => new AudioPlugin(EventHandlerControllerBuilder.CreateEventHandlerController(), pluginId, Logger.ForContext(typeof(AudioPlugin)), eventBus, AudioEventFactory, configuration),
                "IRacingPlugin" => new IRacingPlugin(EventHandlerControllerBuilder.CreateEventHandlerController(), pluginId, IRacingEventFactory, eventBus),
                "TwitchPlugin" => new TwitchPlugin(EventHandlerControllerBuilder.CreateEventHandlerController(), pluginId, Logger.ForContext(typeof(TwitchPlugin)), TwitchEventFactory, eventBus, configuration),
                "TransmitterPlugin" => new TransmitterPlugin(EventHandlerControllerBuilder.CreateEventHandlerController(), pluginId, Logger.ForContext(typeof(TransmitterPlugin)), InternalEventFactory, eventBus, ServiceLocator, configuration),
                "ReceiverPlugin" => new ReceiverPlugin(EventHandlerControllerBuilder.CreateEventHandlerController(), pluginId, Logger.ForContext(typeof(ReceiverPlugin)), InternalEventFactory, eventBus, ServiceLocator, configuration),
                "PlaybackPlugin" => new PlaybackPlugin(EventHandlerControllerBuilder.CreateEventHandlerController(), pluginId, Logger.ForContext(typeof(PlaybackPlugin)), eventBus, ServiceLocator),
                _ => throw new Exception($"Unknown configurable plugin '{name}'"),
            };
        }

        public void Dispose()
        {
            lock (Plugins)
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