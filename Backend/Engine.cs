using Slipstream.Shared;
using Slipstream.Shared.Events.Internal;
using System;
using static Slipstream.Shared.IEventFactory;

#nullable enable

namespace Slipstream.Backend
{
    partial class Engine : Worker, IEngine, IDisposable
    {
        private readonly IEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private readonly IPluginManager PluginManager;
        private readonly IEventBusSubscription Subscription;
        private readonly IPluginFactory PluginFactory;
        private readonly Shared.EventHandler EventHandler = new Shared.EventHandler();

        public Engine(IEventFactory eventFactory, IEventBus eventBus, IPluginFactory pluginFactory, IPluginManager pluginManager) : base("engine")
        {
            EventFactory = eventFactory;
            EventBus = eventBus;
            PluginFactory = pluginFactory;
            PluginManager = pluginManager;

            Subscription = EventBus.RegisterListener();

            EventHandler.OnInternalCommandPluginRegister += (s, e) => OnCommandPluginRegister(e.Event);
            EventHandler.OnInternalCommandPluginUnregister += (s, e) => OnCommandPluginUnregister(e.Event);
            EventHandler.OnInternalCommandPluginEnable += (s, e) => PluginManager.FindPluginAndExecute(e.Event.Id, (plugin) => PluginManager.EnablePlugin(plugin));
            EventHandler.OnInternalCommandPluginDisable += (s, e) => PluginManager.FindPluginAndExecute(e.Event.Id, (plugin) => PluginManager.DisablePlugin(plugin));
            EventHandler.OnInternalCommandPluginStates += (s, e) => OnCommandPluginStates(e.Event);
            EventHandler.OnInternalReconfigured += (s, e) => OnInternalReconfigured();

            // Plugins..
            RegisterPlugin(EventFactory.CreateInternalCommandPluginRegister("FileMonitorPlugin", "FileMonitorPlugin"));
            RegisterPlugin(EventFactory.CreateInternalCommandPluginRegister("FileTriggerPlugin", "FileTriggerPlugin"));
            RegisterPlugin(EventFactory.CreateInternalCommandPluginRegister("AudioPlugin", "AudioPlugin"));
            RegisterPlugin(EventFactory.CreateInternalCommandPluginRegister("IRacingPlugin", "IRacingPlugin"));
            RegisterPlugin(EventFactory.CreateInternalCommandPluginRegister("TwitchPlugin", "TwitchPlugin"));
            RegisterPlugin(EventFactory.CreateInternalCommandPluginRegister("TransmitterPlugin", "TransmitterPlugin"), false);
            RegisterPlugin(EventFactory.CreateInternalCommandPluginRegister("ReceiverPlugin", "ReceiverPlugin"), false);

            // Tell Plugins that we're live - this will make eventbus distribute events
            EventBus.Enabled = true;
        }

        private void OnInternalReconfigured()
        {
            PluginManager.RestartReconfigurablePlugins();
        }

        private void OnCommandPluginStates(InternalCommandPluginStates _)
        {
            PluginManager.ForAllPluginsExecute(
                (a) => EventBus.PublishEvent(
                    EventFactory.CreateInternalPluginState(a.Id, a.Name, a.DisplayName, a.Enabled ? PluginStatusEnum.Enabled : PluginStatusEnum.Disabled)
            ));    
        }

        private void RegisterPlugin(InternalCommandPluginRegister e, bool enable = true)
        {
            OnCommandPluginRegister(e);
            if (enable)
            {
                EventBus.PublishEvent(EventFactory.CreateInternalCommandPluginEnable(e.Id));
            }
        }

        private void OnCommandPluginUnregister(InternalCommandPluginUnregister ev)
        {
            PluginManager.UnregisterPlugin(ev.Id);
        }

        public void UnregisterSubscription(IEventBusSubscription subscription)
        {
            EventBus.UnregisterSubscription(subscription);
        }

        private void OnCommandPluginRegister(Shared.Events.Internal.InternalCommandPluginRegister ev)
        {
            PluginManager.RegisterPlugin(PluginFactory.CreatePlugin(ev.Id, ev.PluginName));
        }

        protected override void Main()
        {
            while (!Stopped)
            {
                EventHandler.HandleEvent(Subscription.NextEvent(10));
            }
        }

        public new void Dispose()
        {
            PluginManager.Dispose();
            base.Dispose();
        }
    }
}