#nullable enable

using Slipstream.Backend.Plugins;
using Slipstream.Shared;
using Slipstream.Shared.Events.Internal;
using System;

namespace Slipstream.Backend
{
    class Engine : Worker, IEngine
    {
        private readonly IEventBus EventBus;
        private readonly PluginManager PluginManager;

        public Engine(IEventBus eventBus)
        {
            EventBus = eventBus;
            PluginManager = new PluginManager(this, eventBus);
        }

        public IEventBusSubscription RegisterListener(IEventListener listener)
        {
            return EventBus.RegisterListener(listener);
        }

        override protected void Main()
        {
            // We need to wait for frontend to be ready, before starting our plugins.
            // This is to avoid that we have plugins that are not shown in the log window
            bool frontendReady = false;

            var subscription = EventBus.RegisterListener("Engine");

            Shared.EventHandler eventHandler = new Shared.EventHandler();
            eventHandler.OnInternalFrontendReady += (s, e) => frontendReady = true;
            eventHandler.OnInternalPluginRegister += (s, e) => OnPluginRegister(e.Event);
            eventHandler.OnDefault += (s, e) => throw new System.Exception($"Unexpect message doring pre-boot: {e.Event.GetType()}");

            while (!frontendReady && !Stopped)
            {
                eventHandler.HandleEvent(subscription.NextEvent(200));
            }

            if (Stopped)
            {
                EventBus.UnregisterListener("Engine");
                return;
            }

            // Frontend is ready, do our part

            PluginManager.EnablePendingPlugins();

            EventBus.PublishEvent(new Shared.Events.Internal.PluginsReady());

            eventHandler = new Shared.EventHandler();
#pragma warning disable CS8604 // Possible null reference argument.
            eventHandler.OnInternalPluginRegister += (s, e) => OnPluginRegister(e.Event);
            eventHandler.OnInternalPluginUnregister += (s, e) => OnPluginUnregister(e.Event);
            eventHandler.OnInternalPluginEnable += (s, e) => PluginManager.FindPluginAndExecute(e.Event.Id, (plugin) => PluginManager.EnablePlugin(plugin));
            eventHandler.OnInternalPluginDisable += (s, e) => PluginManager.FindPluginAndExecute(e.Event.Id, (plugin) => PluginManager.DisablePlugin(plugin));
#pragma warning restore CS8604 // Possible null reference argument.

            while (!Stopped)
            {
                eventHandler.HandleEvent(subscription.NextEvent(500));
            }

            EventBus.UnregisterListener("Engine");

            PluginManager.DisablePlugins();
            PluginManager.UnregisterPlugins();
        }

        private void OnPluginUnregister(PluginUnregister ev)
        {
            PluginManager.UnregisterPlugin(ev.Id);
        }

        public void UnregisterListener(IEventListener listener)
        {
            EventBus.UnregisterListener(listener);
        }

        private void OnPluginRegister(Shared.Events.Internal.PluginRegister ev)
        {
            switch (ev.PluginName)
            {
                case "DebugOutputPlugin":
                    PluginManager.InitializePlugin(new DebugOutputPlugin() { Id = ev.Id }, ev.Enabled != null && ev.Enabled == true);
                    break;
                case "FileMonitorPlugin":
                    if (ev.Settings != null)
                        PluginManager.InitializePlugin(new FileMonitorPlugin(ev.Settings, EventBus) { Id = ev.Id }, ev.Enabled != null && ev.Enabled == true);
                    else
                        throw new Exception($"Missing settings for plugin '{ev.PluginName}'");
                    break;
                case "FileTriggerPlugin":
                    PluginManager.InitializePlugin(new FileTriggerPlugin(EventBus) { Id = ev.Id }, ev.Enabled != null && ev.Enabled == true);
                    break;
                case "LuaPlugin":
                    if (ev.Settings != null)
                        PluginManager.InitializePlugin(new LuaPlugin(ev.Settings) { Id = ev.Id }, ev.Enabled != null && ev.Enabled == true);
                    else
                        throw new Exception($"Missing settings for plugin '{ev.PluginName}'");
                    break;
                default:
                    throw new Exception($"Unknown plugin '{ev.PluginName}'");
            }
        }
    }
}