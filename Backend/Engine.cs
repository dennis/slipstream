using Slipstream.Backend.Plugins;
using Slipstream.Backend.Services;
using Slipstream.Shared;
using Slipstream.Shared.Events;
using Slipstream.Shared.Events.Internal;
using Slipstream.Shared.Events.Setting;
using System;

#nullable enable

namespace Slipstream.Backend
{
    class Engine : Worker, IEngine, IDisposable
    {
        private readonly IEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private readonly PluginManager PluginManager;
        private readonly IEventBusSubscription Subscription;
        private readonly IStateService StateService;
        private readonly IApplicationConfiguration ApplicationConfiguration;
        private readonly Shared.EventHandler EventHandler = new Shared.EventHandler();

        public Engine(IEventFactory eventFactory, IEventBus eventBus, IStateService stateService, IApplicationConfiguration applicationConfiguration) : base("engine")
        {
            EventFactory = eventFactory;
            EventBus = eventBus;
            StateService = stateService;
            ApplicationConfiguration = applicationConfiguration;
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
                    EventFactory.CreateInternalPluginState(a.Id, a.Name, a.DisplayName, a.Enabled ? "Enabled" : "Disabled")
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
            switch (ev.PluginName)
            {
                case "FileMonitorPlugin":
                    {
                        if (!(ev.Settings is FileMonitorSettings settings))
                        {
                            throw new Exception("Unexpected settings for FileMonitorPlugin");
                        }
                        else
                        {
                            PluginManager.RegisterPlugin(new FileMonitorPlugin(ev.Id, EventFactory, EventBus, settings));
                        }
                    }
                    break;
                case "FileTriggerPlugin":
                    PluginManager.RegisterPlugin(new FileTriggerPlugin(ev.Id, EventFactory, EventBus));
                    break;
                case "LuaPlugin":
                    {
                        if (!(ev.Settings is LuaSettings settings))
                        {
                            throw new Exception("Unexpected settings for LuaPlugin");
                        }
                        else
                        {
                            PluginManager.RegisterPlugin(new LuaPlugin(ev.Id, EventFactory, EventBus, StateService, settings));
                        }
                    }
                    break;
                case "AudioPlugin":
                    {
                        if (!(ev.Settings is AudioSettings settings))
                        {
                            throw new Exception("Unexpected settings for AudioPlugin");
                        }
                        else
                        {
                            PluginManager.RegisterPlugin(new AudioPlugin(ev.Id, EventFactory, EventBus, settings));
                        }
                    }
                    break;
                case "IRacingPlugin":
                    PluginManager.RegisterPlugin(new IRacingPlugin(ev.Id, EventFactory, EventBus));
                    break;
                case "TwitchPlugin":
                    {
                        if (!(ev.Settings is TwitchSettings settings))
                        {
                            throw new Exception("Unexpected settings for TwitchPlugin");
                        }
                        else
                        {
                            PluginManager.RegisterPlugin(new TwitchPlugin(ev.Id, EventFactory, EventBus, settings));
                        }
                    }
                    break;
                case "TransmitterPlugin":
                    {
                        if (!(ev.Settings is TxrxSettings settings))
                        {
                            throw new Exception("Unexpected settings for TransmitterPlugin");
                        }
                        else
                        {
                            PluginManager.RegisterPlugin(new TransmitterPlugin(ev.Id, EventFactory, EventBus, settings));
                        }
                    }
                    break;
                case "ReceiverPlugin":
                    {
                        if (!(ev.Settings is TxrxSettings settings))
                        {
                            throw new Exception("Unexpected settings for ReceiverPlugin");
                        }
                        else
                        {
                            PluginManager.RegisterPlugin(new ReceiverPlugin(ev.Id, EventFactory, EventBus, settings));
                        }
                    }
                    break;
                default:
                    throw new Exception($"Unknown plugin '{ev.PluginName}'");
            }
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