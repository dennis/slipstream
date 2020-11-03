#nullable enable

using Slipstream.Backend.Plugins;
using Slipstream.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Slipstream.Backend
{
    class Engine : Worker, IEngine
    {
        private readonly List<IPlugin> Plugins;
        private readonly IEventBus EventBus;

        public Engine(IEventBus eventBus)
        {
            EventBus = eventBus;
            Plugins = new List<IPlugin>();
        }

        public IEventBusSubscription RegisterListener(IEventListener listener)
        {
            return EventBus.RegisterListener(listener);
        }

        public void UnregisterListener(IEventListener listener)
        {
            EventBus.UnregisterListener(listener);
        }

        private void EmitePluginStateChanged(IPlugin plugin, Shared.Events.Internal.PluginStatus status)
        {
            EventBus.PublishEvent(new Shared.Events.Internal.PluginStateChanged() { PluginName = plugin.Name, PluginStatus = status });
        }

        private void RegisterPlugin(IPlugin p)
        {
            p.RegisterPlugin(this);
            EmitePluginStateChanged(p, Shared.Events.Internal.PluginStatus.Registered);
        }

        private void UnregisterPlugins()
        {
            foreach (var p in Plugins)
            {
                UnregisterPlugin(p);
            }
        }
        private void UnregisterPlugin(IPlugin p)
        {
            p.UnregisterPlugin(this);
            EmitePluginStateChanged(p, Shared.Events.Internal.PluginStatus.Unregistered);
        }

        private void EnablePlugin(IPlugin p)
        {
            p.Enable(this);
            EmitePluginStateChanged(p, Shared.Events.Internal.PluginStatus.Enabled);
        }

        private void DisablePlugins()
        {
            foreach (var p in Plugins)
            {
                DisablePlugin(p);
            }
        }

        private void DisablePlugin(IPlugin p)
        {
            p.Disable(this);
            EmitePluginStateChanged(p, Shared.Events.Internal.PluginStatus.Disabled);
        }

        override protected void Main()
        {
            // We need to wait for frontend to be ready, before starting our plugins.
            // This is to avoid that we have plugins that are not shown in the log window
            bool frontendReady = false;

            var subscription = EventBus.RegisterListener("Engine");

            while (!frontendReady && !Stopped)
            {
                IEvent? e = subscription.NextEvent(200);
                if(e == null)
                    continue;

                 switch(e)
                {
                    case Shared.Events.Internal.FrontendReady _:
                        frontendReady = true;
                        break;
                    case Shared.Events.Internal.PluginLoad ev:
                        OnLoadPlugin(ev);
                        break;
                    default:
                        throw new System.Exception($"Unexpect message doring pre-boot: {e.GetType()}");
                };
            }


            if (Stopped)
            {
                EventBus.UnregisterListener("Engine");
                return;
            }

            // Frontend is ready, do our part

            while (!Stopped)
            {
                var e = subscription.NextEvent(1000);
                if(e != null)
                {
                    switch(e)
                    {
                        case Shared.Events.Internal.PluginEnable ev:
                            foreach (var p in Plugins)
                                if (p.Name == ev.PluginName)
                                    EnablePlugin(p);
                            break;
                        case Shared.Events.Internal.PluginDisable ev:
                            foreach (var p in Plugins)
                                if (p.Name == ev.PluginName)
                                    DisablePlugin(p);
                            break;
                    }
                }
            }

            EventBus.UnregisterListener("Engine");

            DisablePlugins();
            UnregisterPlugins();
        }

        private void OnLoadPlugin(Shared.Events.Internal.PluginLoad e)
        {
            switch(e.PluginName)
            {
                case "DebugOutputPlugin":
                    var plugin = new DebugOutputPlugin();
                    Plugins.Add(plugin);
                    RegisterPlugin(plugin);
                    if (e.Enabled != null && e.Enabled == true)
                        EnablePlugin(plugin);
                    break;

                default:
                    Debug.WriteLine("Unexpected pluginname ${e.PluginName}");
                    break;
            }
        }
    }
}
