#nullable enable

using Serilog;
using Slipstream.Backend.Plugins;
using Slipstream.Components;
using Slipstream.Components.FileMonitor;
using Slipstream.Shared;
using Slipstream.Shared.Factories;
using Slipstream.Shared.Helpers.StrongParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using static Slipstream.Shared.Factories.IInternalEventFactory;

namespace Slipstream.Backend
{
    public class PluginManager : IPluginManager, IPluginFactory
    {
        private readonly IInternalEventFactory InternalEventFactory;
        private readonly ITwitchEventFactory TwitchEventFactory;
        private readonly IServiceLocator ServiceLocator;

        private readonly IEventBus EventBus;
        private readonly IDictionary<string, PluginWorker> PluginWorkers = new Dictionary<string, PluginWorker>();
        private readonly ILogger Logger;
        private readonly EventHandlerControllerBuilder EventHandlerControllerBuilder;
        private readonly ComponentRegistrator Registrator;
        private readonly List<ILuaGlue> LuaGlues = new List<ILuaGlue>();
        private readonly Dictionary<string, Func<IComponentPluginCreationContext, IPlugin>> ComponentPlugins = new Dictionary<string, Func<IComponentPluginCreationContext, IPlugin>>();

        public PluginManager(
            IEventFactory eventFactory,
            IEventBus eventBus,
            IServiceLocator serviceLocator,
            ILogger logger,
            EventHandlerControllerBuilder eventHandlerControllerBuilder)
        {
            Registrator = new ComponentRegistrator(ComponentPlugins, LuaGlues, eventFactory, logger, eventBus, eventHandlerControllerBuilder, serviceLocator);

            foreach (var type in typeof(PluginManager).Assembly.GetTypes().Where(t => typeof(IComponent).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass))
            {
                logger.Information("Initializing component {pluginName}", type.Name);
                ((IComponent)Activator.CreateInstance(type)).Register(Registrator);
            }

            InternalEventFactory = eventFactory.Get<IInternalEventFactory>();
            TwitchEventFactory = eventFactory.Get<ITwitchEventFactory>();
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
                "TwitchPlugin" => new TwitchPlugin(EventHandlerControllerBuilder.CreateEventHandlerController(), pluginId, Logger.ForContext(typeof(TwitchPlugin)), TwitchEventFactory, eventBus, configuration),
                "TransmitterPlugin" => new TransmitterPlugin(EventHandlerControllerBuilder.CreateEventHandlerController(), pluginId, Logger.ForContext(typeof(TransmitterPlugin)), InternalEventFactory, eventBus, ServiceLocator, configuration),
                "ReceiverPlugin" => new ReceiverPlugin(EventHandlerControllerBuilder.CreateEventHandlerController(), pluginId, Logger.ForContext(typeof(ReceiverPlugin)), InternalEventFactory, eventBus, ServiceLocator, configuration),
                "PlaybackPlugin" => new PlaybackPlugin(EventHandlerControllerBuilder.CreateEventHandlerController(), pluginId, Logger.ForContext(typeof(PlaybackPlugin)), eventBus, ServiceLocator),
                _ => CreateViaComponents(pluginId, name, configuration)
            };
        }

        private IPlugin CreateViaComponents(string pluginId, string pluginName, Parameters configuration)
        {
            ComponentPluginCreationContext reg = new ComponentPluginCreationContext(Registrator, this, this, LuaGlues, pluginId, pluginName, configuration);
            return ComponentPlugins[pluginName].Invoke(reg);
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