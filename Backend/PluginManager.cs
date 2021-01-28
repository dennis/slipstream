#nullable enable

using Serilog;
using Slipstream.Backend.Plugins;
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

        public void UnregisterPlugin(IPlugin p)
        {
            lock (PluginWorkers)
            {
                UnregisterPluginWithoutLock(p.Id);
            }
        }

        private void EmitPluginStateChanged(IPlugin plugin, PluginStatusEnum pluginStatus)
        {
            EmitEvent(InternalEventFactory.CreateInternalPluginState(plugin.Id, plugin.Name, plugin.DisplayName, pluginStatus));
        }

        public void RegisterPlugin(IPlugin plugin)
        {
            var worker = new PluginWorker(plugin, EventBus.RegisterListener(), InternalEventFactory, EventBus);

            lock (PluginWorkers)
            {
                PluginWorkers.Add(plugin.Id, worker);

                worker.Start();
            }

            EmitPluginStateChanged(plugin, PluginStatusEnum.Registered);
            Logger.Verbose("Started for {PluginId}", plugin.Id);
        }

        private void EmitEvent(IEvent e)
        {
            EventBus.PublishEvent(e);
        }

        public void FindPluginAndExecute(string pluginId, Action<IPlugin> a)
        {
            lock (PluginWorkers)
            {
                if (PluginWorkers.TryGetValue(pluginId, out PluginWorker worker))
                {
                    a(worker.Plugin);
                }
                else
                {
                    Logger.Error("Can't find plugin {pluginId}", pluginId);
                }
            }
        }

        public void ForAllPluginsExecute(Action<IPlugin> a)
        {
            lock (PluginWorkers)
            {
                foreach (var p in PluginWorkers)
                {
                    a(p.Value.Plugin);
                }
            }
        }

        public void UnregisterPlugin(string id)
        {
            lock (PluginWorkers)
            {
                UnregisterPluginWithoutLock(id);
            }
        }

        private void UnregisterPluginWithoutLock(string id)
        {
            lock (PluginWorkers)
            {
                if (PluginWorkers.ContainsKey(id))
                {
                    PluginWorkers[id].Stop();
                    EmitPluginStateChanged(PluginWorkers[id].Plugin, PluginStatusEnum.Unregistered);

                    PluginWorkers.Remove(id);
                    Logger.Verbose("Removed plugin: {pluginId}", id);
                }
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
            lock (PluginWorkers)
            {
                foreach (var worker in PluginWorkers)
                {
                    worker.Value.Stop();
                    worker.Value.Dispose();
                }
            }
        }
    }
}