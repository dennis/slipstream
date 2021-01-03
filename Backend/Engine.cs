using Slipstream.Backend.Services;
using Slipstream.Shared;
using Slipstream.Shared.Events;
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
        private readonly PluginManager PluginManager;
        private readonly IEventBusSubscription Subscription;
        private readonly IApplicationConfiguration ApplicationConfiguration;
        private readonly PluginFactory PluginFactory;
        private readonly Shared.EventHandler EventHandler = new Shared.EventHandler();

        public Engine(IEventFactory eventFactory, IEventBus eventBus, IApplicationConfiguration applicationConfiguration, PluginFactory pluginFactory) : base("engine")
        {
            EventFactory = eventFactory;
            EventBus = eventBus;
            ApplicationConfiguration = applicationConfiguration;
            PluginFactory = pluginFactory;

            PluginManager = new PluginManager(this, eventFactory, eventBus);

            Subscription = EventBus.RegisterListener();

            EventHandler.OnInternalCommandPluginRegister += (s, e) => OnCommandPluginRegister(e.Event);
            EventHandler.OnInternalCommandPluginUnregister += (s, e) => OnCommandPluginUnregister(e.Event);
            EventHandler.OnInternalCommandPluginEnable += (s, e) => PluginManager.FindPluginAndExecute(e.Event.Id, (plugin) => PluginManager.EnablePlugin(plugin));
            EventHandler.OnInternalCommandPluginDisable += (s, e) => PluginManager.FindPluginAndExecute(e.Event.Id, (plugin) => PluginManager.DisablePlugin(plugin));
            EventHandler.OnInternalCommandPluginStates += (s, e) => OnCommandPluginStates(e.Event);

            // Plugins..
            RegisterPlugin(EventFactory.CreateInternalCommandPluginRegister("FileMonitorPlugin", "FileMonitorPlugin", ApplicationConfiguration.GetFileMonitorSettingsEvent()));
            RegisterPlugin(EventFactory.CreateInternalCommandPluginRegister("FileTriggerPlugin", "FileTriggerPlugin"));
            RegisterPlugin(EventFactory.CreateInternalCommandPluginRegister("AudioPlugin", "AudioPlugin", ApplicationConfiguration.GetAudioSettingsEvent()));
            RegisterPlugin(EventFactory.CreateInternalCommandPluginRegister("IRacingPlugin", "IRacingPlugin"));
            RegisterPlugin(EventFactory.CreateInternalCommandPluginRegister("TwitchPlugin", "TwitchPlugin", ApplicationConfiguration.GetTwitchSettingsEvent()));
            RegisterPlugin(EventFactory.CreateInternalCommandPluginRegister("TransmitterPlugin", "TransmitterPlugin", ApplicationConfiguration.GetTxrxSettingsEvent()), false);
            RegisterPlugin(EventFactory.CreateInternalCommandPluginRegister("ReceiverPlugin", "ReceiverPlugin", ApplicationConfiguration.GetTxrxSettingsEvent()), false);

            // Tell Plugins that we're live - this will make eventbus distribute events
            EventBus.Enabled = true;
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

        public IEventBusSubscription RegisterListener()
        {
            return EventBus.RegisterListener();
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

            PluginManager.RegisterPlugin(PluginFactory.CreatePlugin(ev.Id, ev.PluginName, ev.Settings));
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