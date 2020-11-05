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
        private readonly IDictionary<string, IPlugin> Plugins;
        private readonly IEventBus EventBus;

        public Engine(IEventBus eventBus)
        {
            EventBus = eventBus;
            Plugins = new Dictionary<string, IPlugin>();
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
                UnregisterPlugin(p.Value);
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
                DisablePlugin(p.Value);
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

                switch (e)
                {
                    case Shared.Events.Internal.FrontendReady _:
                        frontendReady = true;
                        break;
                    case Shared.Events.Internal.PluginLoad ev:
                        OnPluginLoad(ev);
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
#pragma warning disable CS8604 // Possible null reference argument.
                            FindPluginAndExecute(ev.PluginName, (plugin) => EnablePlugin(plugin));
                            break;
                        case Shared.Events.Internal.PluginDisable ev:
                            FindPluginAndExecute(ev.PluginName, (plugin) => DisablePlugin(plugin));
#pragma warning restore CS8604 // Possible null reference argument.
                            break;
                    }
                }
            }

            EventBus.UnregisterListener("Engine");

            DisablePlugins();
            UnregisterPlugins();
        }

        private void OnPluginLoad(Shared.Events.Internal.PluginLoad ev)
        {
            switch(ev.PluginName)
            {
                case "DebugOutputPlugin":
                    InitializePlugin(new DebugOutputPlugin(), ev.Enabled != null && ev.Enabled == true);
                    break;
                case "FileMonitorPlugin":
                    if(ev.Settings != null)
                        InitializePlugin(new FileMonitorPlugin(ev.Settings, EventBus), ev.Enabled != null && ev.Enabled == true);
                    else
                        throw new Exception($"Missing settings for plugin '{ev.PluginName}'");
                    break;
                default:
                    throw new Exception($"Unknown plugin '{ev.PluginName}'");
            }
        }

        private void FindPluginAndExecute(string pluginName, Action<IPlugin> a)
        {
            if (Plugins.TryGetValue(pluginName, out IPlugin plugin))
            {
                a(plugin);
            }
            else
            {
                Debug.WriteLine($"Plugin not found '{pluginName}'");
            }
        }

        private void InitializePlugin(IPlugin plugin, bool enabled)
        {
            Plugins.Add(plugin.Name, plugin);
            RegisterPlugin(plugin);
            if (enabled)
                EnablePlugin(plugin);
        }
    }
}