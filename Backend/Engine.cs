using Newtonsoft.Json.Linq;
using Serilog;
using Slipstream.Components.Internal;
using Slipstream.Components.Internal.Events;
using Slipstream.Components.Internal.Services;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using System;
using System.IO;

#nullable enable

namespace Slipstream.Backend
{
    internal class Engine : IEngine, IDisposable
    {
        private readonly IInternalEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private readonly IPluginManager PluginManager;
        private readonly IEventBusSubscription Subscription;
        private readonly IPluginFactory PluginFactory;
        private readonly ILogger Logger;
        private readonly IEventHandlerController EventHandlerController;
        private bool Stopped = false;

        public Engine(ILogger logger, IEventFactory eventFactory, IEventBus eventBus, IPluginFactory pluginFactory, IPluginManager pluginManager, EventHandlerControllerBuilder eventHandlerControllerBuilder)
        {
            EventFactory = eventFactory.Get<IInternalEventFactory>();
            EventBus = eventBus;
            PluginFactory = pluginFactory;
            PluginManager = pluginManager;
            Logger = logger;
            EventHandlerController = eventHandlerControllerBuilder.CreateEventHandlerController();

            Subscription = EventBus.RegisterListener();

            var internalEventHandler = EventHandlerController.Get<Slipstream.Components.Internal.EventHandler.Internal>();

            internalEventHandler.OnInternalCommandPluginRegister += (_, e) => OnCommandPluginRegister(e);
            internalEventHandler.OnInternalCommandPluginUnregister += (_, e) => OnCommandPluginUnregister(e);
            internalEventHandler.OnInternalCommandPluginStates += (_, e) => OnCommandPluginStates(e);
            internalEventHandler.OnInternalCommandShutdown += (_, e) => OnInternalCommandShutdown(e);

            // Plugins..
            {
                const string initFilename = "init.lua";

                if (!File.Exists(initFilename))
                {
                    Logger.Information("No {initcfg} file found, creating", initFilename);
                    CreateInitLua(initFilename);
                }

                Logger.Information("Loading {initcfg}", initFilename);

                // FIXME: This needs to be reimplemented
                var luaService = new LuaService(new System.Collections.Generic.List<Components.ILuaGlue> { new Slipstream.Components.Internal.LuaGlues.InternalLuaGlue(eventBus, EventFactory) });
                luaService.Parse(initFilename);
            }

            // Tell Plugins that we're live - this will make eventbus distribute events
            EventBus.Enabled = true;
        }

        private void OnInternalCommandShutdown(InternalCommandShutdown _)
        {
            EventBus.PublishEvent(EventFactory.CreateInternalShutdown());
            Stopped = true;
        }

        private void CreateInitLua(string initFilename)
        {
            var assembly = this.GetType().Assembly;
            using var initLuaStream = assembly.GetManifestResourceStream("Slipstream.Backend.Bootstrap.init.lua");
            using var sr = new StreamReader(initLuaStream);
            var initLuaContent = sr.ReadToEnd();

            File.WriteAllText(initFilename, initLuaContent);
        }

        private void OnCommandPluginStates(InternalCommandPluginStates _)
        {
            PluginManager.ForAllPluginsExecute(
                (a) => EventBus.PublishEvent(
                    EventFactory.CreateInternalPluginState(a.Id, a.Name, a.DisplayName, IInternalEventFactory.PluginStatusEnum.Registered)
            ));
        }

        private void OnCommandPluginUnregister(InternalCommandPluginUnregister ev)
        {
            PluginManager.UnregisterPlugin(ev.Id);
        }

        public void UnregisterSubscription(IEventBusSubscription subscription)
        {
            EventBus.UnregisterSubscription(subscription);
        }

        private void OnCommandPluginRegister(InternalCommandPluginRegister ev)
        {
            JObject a = JObject.Parse(ev.Configuration);
            Parameters configuration = Parameters.From(a);

            try
            {
                PluginManager.RegisterPlugin(PluginFactory.CreatePlugin(ev.Id, ev.PluginName, configuration));
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Failed creating plugin '{ev.Id}' ('{ev.PluginName}'): {e}");
            }
        }

        public void Start()
        {
            while (!Stopped)
            {
                EventHandlerController.HandleEvent(Subscription.NextEvent(10));
            }
        }

        public void Dispose()
        {
            PluginManager.Dispose();
        }
    }
}