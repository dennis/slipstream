#nullable enable

using Slipstream.Backend.Plugins;
using Slipstream.Shared;
using Slipstream.Shared.Events.Internal;
using System;

namespace Slipstream.Backend
{
    class Engine : Worker, IEngine, IDisposable
    {
        private readonly IEventBus EventBus;
        private readonly PluginManager PluginManager;
        private readonly IEventBusSubscription Subscription;
        private bool FrontendReady = false;

        // Before UI is ready:
        private readonly Shared.EventHandler PreEventHandler = new Shared.EventHandler();

        // After UI is ready
        private readonly Shared.EventHandler PostEventHandler = new Shared.EventHandler();

        public Engine(IEventBus eventBus) : base("engine")
        {
            EventBus = eventBus;
            PluginManager = new PluginManager(this, eventBus);

            Subscription = EventBus.RegisterListener();

            PreEventHandler.OnInternalFrontendReady += (s, e) => OnFrontendReady(e.Event);
            PreEventHandler.OnInternalPluginRegister += (s, e) => OnPluginRegister(e.Event);
            PreEventHandler.OnDefault += (s, e) => throw new System.Exception($"Unexpect message doring pre-boot: {e.Event.GetType()}");


#pragma warning disable CS8604 // Possible null reference argument.
            PostEventHandler.OnInternalPluginRegister += (s, e) => OnPluginRegister(e.Event);
            PostEventHandler.OnInternalPluginUnregister += (s, e) => OnPluginUnregister(e.Event);
            PostEventHandler.OnInternalPluginEnable += (s, e) => PluginManager.FindPluginAndExecute(e.Event.Id, (plugin) => PluginManager.EnablePlugin(plugin));
            PostEventHandler.OnInternalPluginDisable += (s, e) => PluginManager.FindPluginAndExecute(e.Event.Id, (plugin) => PluginManager.DisablePlugin(plugin));
#pragma warning restore CS8604 // Possible null reference argument.

        }

        private void OnFrontendReady(FrontendReady _)
        {
            FrontendReady = true;

            PluginManager.EnablePendingPlugins();

            EventBus.PublishEvent(new Shared.Events.Internal.PluginsReady());
        }

        public IEventBusSubscription RegisterListener()
        {
            return EventBus.RegisterListener();
        }

        private void OnPluginUnregister(PluginUnregister ev)
        {
            PluginManager.UnregisterPlugin(ev.Id);
        }

        public void UnregisterSubscription(IEventBusSubscription subscription)
        {
            EventBus.UnregisterSubscription(subscription);
        }

        private void OnPluginRegister(Shared.Events.Internal.PluginRegister ev)
        {
            switch (ev.PluginName)
            {
                case "DebugOutputPlugin":
                    PluginManager.InitializePlugin(new DebugOutputPlugin(ev.Id), ev.Enabled);
                    break;
                case "FileMonitorPlugin":
                    if (ev.Settings != null)
                        PluginManager.InitializePlugin(new FileMonitorPlugin(ev.Id, ev.Settings, EventBus), ev.Enabled);
                    else
                        throw new Exception($"Missing settings for plugin '{ev.PluginName}'");
                    break;
                case "FileTriggerPlugin":
                    PluginManager.InitializePlugin(new FileTriggerPlugin(ev.Id, EventBus), ev.Enabled);
                    break;
                case "LuaPlugin":
                    if (ev.Settings != null)
                        PluginManager.InitializePlugin(new LuaPlugin(ev.Id, ev.Settings, EventBus), ev.Enabled);
                    else
                        throw new Exception($"Missing settings for plugin '{ev.PluginName}'");
                    break;
                case "AudioPlugin":
                    if (ev.Settings != null)
                        PluginManager.InitializePlugin(new AudioPlugin(ev.Id, ev.Settings, EventBus), ev.Enabled);
                    else
                        throw new Exception($"Missing settings for plugin '{ev.PluginName}'");
                    break;
                case "IRacingPlugin":
                    PluginManager.InitializePlugin(new IRacingPlugin(ev.Id, EventBus), ev.Enabled);
                    break;
                default:
                    throw new Exception($"Unknown plugin '{ev.PluginName}'");
            }
        }

        protected override void Main()
        {
            while (!Stopped)
            {
                if (!FrontendReady)
                {
                    // We need to wait for frontend to be ready, before starting our plugins.
                    // This is to avoid that we have plugins that are not shown in the log window

                    PreEventHandler.HandleEvent(Subscription.NextEvent(200));
                }
                else
                {
                    // Frontend is ready, do our part

                    PostEventHandler.HandleEvent(Subscription.NextEvent(200));
                }
            }
        }

        public new void Dispose()
        {
            PluginManager.Dispose();
            base.Dispose();
        }
    }
}