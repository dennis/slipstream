using Slipstream.Backend.Plugins;
using Slipstream.Backend.Services;
using Slipstream.Shared;
using Slipstream.Shared.Events.Internal;
using Slipstream.Shared.Events.Setting;
using System;

#nullable enable

namespace Slipstream.Backend
{
    class Engine : Worker, IEngine, IDisposable
    {
        private readonly IEventBus EventBus;
        private readonly PluginManager PluginManager;
        private readonly IEventBusSubscription Subscription;
        private readonly IStateService StateService;
        private readonly IApplicationConfiguration ApplicationConfiguration;
        private readonly Shared.EventHandler EventHandler = new Shared.EventHandler();

        public Engine(IEventBus eventBus, IStateService stateService, IApplicationConfiguration applicationConfiguration) : base("engine")
        {
            EventBus = eventBus;
            StateService = stateService;
            ApplicationConfiguration = applicationConfiguration;
            PluginManager = new PluginManager(this, eventBus);

            Subscription = EventBus.RegisterListener();

            EventHandler.OnInternalCommandPluginRegister += (s, e) => OnCommandPluginRegister(e.Event);
            EventHandler.OnInternalCommandPluginUnregister += (s, e) => OnCommandPluginUnregister(e.Event);
            EventHandler.OnInternalCommandPluginEnable += (s, e) => PluginManager.FindPluginAndExecute(e.Event.Id, (plugin) => PluginManager.EnablePlugin(plugin));
            EventHandler.OnInternalCommandPluginDisable += (s, e) => PluginManager.FindPluginAndExecute(e.Event.Id, (plugin) => PluginManager.DisablePlugin(plugin));
            EventHandler.OnInternalCommandPluginStates += (s, e) => OnCommandPluginStates(e.Event);

            // Plugins..
            RegisterPlugin(new Shared.Events.Internal.CommandPluginRegister() { Id = "FileMonitorPlugin", PluginName = "FileMonitorPlugin", Settings = ApplicationConfiguration.GetFileMonitorSettingsEvent() });
            RegisterPlugin(new Shared.Events.Internal.CommandPluginRegister() { Id = "FileTriggerPlugin", PluginName = "FileTriggerPlugin" });
            RegisterPlugin(new Shared.Events.Internal.CommandPluginRegister() { Id = "AudioPlugin", PluginName = "AudioPlugin", Settings = ApplicationConfiguration.GetAudioSettingsEvent() });
            RegisterPlugin(new Shared.Events.Internal.CommandPluginRegister() { Id = "IRacingPlugin", PluginName = "IRacingPlugin" });
            RegisterPlugin(new Shared.Events.Internal.CommandPluginRegister() { Id = "TwitchPlugin", PluginName = "TwitchPlugin", Settings = ApplicationConfiguration.GetTwitchSettingsEvent() });

            // Tell Plugins that we're live - this will make eventbus distribute events
            EventBus.Enabled = true;
            EventBus.PublishEvent(new Shared.Events.Internal.PluginsReady());
        }

        private void OnCommandPluginStates(CommandPluginStates _)
        {
            PluginManager.ForAllPluginsExecute(
                (a) => EventBus.PublishEvent(
                    new Shared.Events.Internal.PluginState
                    {
                        Id = a.Id,
                        DisplayName = a.DisplayName,
                        PluginName = a.Name,
                        PluginStatus = a.Enabled ? "Enabled" : "Disabled"
                    }));
        }

        private void RegisterPlugin(CommandPluginRegister e)
        {
            OnCommandPluginRegister(e);
            EventBus.PublishEvent(new Shared.Events.Internal.CommandPluginEnable() { Id = e.Id });
        }

        public IEventBusSubscription RegisterListener()
        {
            return EventBus.RegisterListener();
        }

        private void OnCommandPluginUnregister(CommandPluginUnregister ev)
        {
            PluginManager.UnregisterPlugin(ev.Id);
        }

        public void UnregisterSubscription(IEventBusSubscription subscription)
        {
            EventBus.UnregisterSubscription(subscription);
        }

        private void OnCommandPluginRegister(Shared.Events.Internal.CommandPluginRegister ev)
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
                            PluginManager.RegisterPlugin(new FileMonitorPlugin(ev.Id, EventBus, settings));
                        }
                    }
                    break;
                case "FileTriggerPlugin":
                    PluginManager.RegisterPlugin(new FileTriggerPlugin(ev.Id, EventBus));
                    break;
                case "LuaPlugin":
                    {
                        if (!(ev.Settings is LuaSettings settings))
                        {
                            throw new Exception("Unexpected settings for LuaPlugin");
                        }
                        else
                        {
                            PluginManager.RegisterPlugin(new LuaPlugin(ev.Id, EventBus, StateService, settings));
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
                            PluginManager.RegisterPlugin(new AudioPlugin(ev.Id, EventBus, settings));
                        }
                    }
                    break;
                case "IRacingPlugin":
                    PluginManager.RegisterPlugin(new IRacingPlugin(ev.Id, EventBus));
                    break;
                case "TwitchPlugin":
                    {
                        if (!(ev.Settings is TwitchSettings settings))
                        {
                            throw new Exception("Unexpected settings for TwitchPlugin");
                        }
                        else
                        {
                            PluginManager.RegisterPlugin(new TwitchPlugin(ev.Id, EventBus, settings));
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