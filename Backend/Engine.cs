#nullable enable

using Slipstream.Backend.Plugins;
using Slipstream.Shared;
using Slipstream.Shared.Events.Internal;
using System.Collections.Generic;

namespace Slipstream.Backend
{
    class Engine : Worker, IEngine
    {
        private readonly List<IPlugin> Plugins;
        private readonly IEventBus EventBus;

        public Engine(IEventBus eventBus)
        {
            EventBus = eventBus;
            Plugins = new List<IPlugin>
            {
                new DebugOutputPlugin(),
            };
        }

        public IEventBusSubscription RegisterListener(IEventListener listener)
        {
            return EventBus.RegisterListener(listener);
        }

        public void UnregisterListener(IEventListener listener)
        {
            EventBus.UnregisterListener(listener);
        }

        private void EmitePluginStateChanged(IPlugin plugin, PluginStatus status)
        {
            EventBus.PublishEvent(new PluginStateChanged() { PluginName = plugin.Name, PluginStatus = status });
        }

        private void RegisterPlugins()
        {
            foreach (var p in Plugins)
            {
                RegisterPlugin(p);
            }
        }

        private void RegisterPlugin(IPlugin p)
        {
            p.RegisterPlugin(this);
            EmitePluginStateChanged(p, PluginStatus.Registered);
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
            EmitePluginStateChanged(p, PluginStatus.Unregistered);
        }

        private void EnablePlugins()
        {
            foreach (var p in Plugins)
            {
                EnablePlugin(p);
            }
        }

        private void EnablePlugin(IPlugin p)
        {
            p.Enable(this);
            EmitePluginStateChanged(p, PluginStatus.Enabled);
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
            EmitePluginStateChanged(p, PluginStatus.Disabled);
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

                frontendReady = e switch
                {
                    FrontendReady _ => true,
                    _ => throw new System.Exception($"Unexpect message doring pre-boot: {e.GetType()}"),
                };
            }


            if (Stopped)
            {
                EventBus.UnregisterListener("Engine");
                return;
            }

            // Frontend is ready, do our part

            RegisterPlugins();
            EnablePlugins();

            while (!Stopped)
            {
                var e = subscription.NextEvent(1000);
                if(e != null)
                {
                    switch(e)
                    {
                        case PluginEnable ev:
                            foreach (var p in Plugins)
                                if (p.Name == ev.PluginName)
                                    EnablePlugin(p);
                            break;
                        case PluginDisable ev:
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
    }
}
