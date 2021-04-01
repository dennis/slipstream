#nullable enable

using Autofac;
using Serilog;
using Slipstream.Components;
using Slipstream.Components.AppilcationUpdate;
using Slipstream.Components.Audio;
using Slipstream.Components.Discord;
using Slipstream.Components.FileMonitor;
using Slipstream.Components.Internal;
using Slipstream.Components.IRacing;
using Slipstream.Components.Lua;
using Slipstream.Components.Playback;
using Slipstream.Components.Twitch;
using Slipstream.Components.UI;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using static Slipstream.Components.Internal.IInternalEventFactory;

namespace Slipstream.Backend
{
    public class PluginManager : IPluginManager, IPluginFactory
    {
        private readonly IInternalEventFactory InternalEventFactory;
        private readonly IEventSerdeService EventSerdeService;
        private readonly IEventBus EventBus;
        private readonly IDictionary<string, PluginWorker> PluginWorkers = new Dictionary<string, PluginWorker>();
        private readonly ILogger Logger;
        private readonly ILifetimeScope LifetimeScope;
        private readonly ComponentRegistrator Registrator;
        private readonly List<ILuaGlueFactory> LuaGluesFactories = new List<ILuaGlueFactory>();
        private readonly Dictionary<string, Func<IComponentPluginCreationContext, IPlugin>> ComponentPlugins = new Dictionary<string, Func<IComponentPluginCreationContext, IPlugin>>();

        public PluginManager(
            IInternalEventFactory internalEventFactory,
            IEventBus eventBus,
            ILogger logger,
            IEventSerdeService eventSerdeService,
            ILifetimeScope lifetimeScope
        )
        {
            Registrator = new ComponentRegistrator(
                ComponentPlugins,
                LuaGluesFactories,
                logger,
                eventBus);

            foreach (var type in typeof(PluginManager).Assembly.GetTypes().Where(t => typeof(IComponent).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass))
            {
                logger.Information("Initializing component {pluginName}", type.Name);
                ((IComponent)Activator.CreateInstance(type)).Register(Registrator);
            }

            InternalEventFactory = internalEventFactory;
            EventSerdeService = eventSerdeService;
            EventBus = eventBus;
            Logger = logger;
            LifetimeScope = lifetimeScope;
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
            Logger.Verbose("Started plugin: {PluginName}: {PluginId}", plugin.Name, plugin.Id);
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

        public IPlugin CreatePlugin(string pluginId, string pluginName, IEventBus eventBus, Parameters configuration)
        {
            ComponentPluginCreationContext reg = new ComponentPluginCreationContext(
                Registrator,
                this,
                this,
                LuaGluesFactories,
                pluginId,
                pluginName,
                configuration,
                EventSerdeService,
                LifetimeScope.Resolve<IEventHandlerController>(),
                LifetimeScope.Resolve<IInternalEventFactory>(),
                LifetimeScope.Resolve<IUIEventFactory>(),
                LifetimeScope.Resolve<IPlaybackEventFactory>(),
                LifetimeScope.Resolve<ILuaEventFactory>(),
                LifetimeScope.Resolve<IApplicationUpdateEventFactory>(),
                LifetimeScope.Resolve<IFileMonitorEventFactory>(),
                LifetimeScope.Resolve<IAudioEventFactory>(),
                LifetimeScope.Resolve<IDiscordEventFactory>(),
                LifetimeScope.Resolve<IIRacingEventFactory>(),
                LifetimeScope.Resolve<ITwitchEventFactory>()
            );
            if (!ComponentPlugins.ContainsKey(pluginName))
                throw new KeyNotFoundException($"Plugin name '{pluginName}' not found");
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