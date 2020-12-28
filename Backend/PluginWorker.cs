using Slipstream.Shared;
using Slipstream.Shared.Events.Internal;
using System.Collections.Generic;
using System.Threading;

#nullable enable

namespace Slipstream.Backend
{
    class PluginWorker : Worker
    {
        private readonly IList<IPlugin> Plugins = new List<IPlugin>();
        private readonly IEventBusSubscription Subscription;
        private readonly IEventBus EventBus;

        public PluginWorker(string name, IEventBusSubscription subscription, IEventBus eventBus) : base(name)
        {
            Subscription = subscription;
            EventBus = eventBus;
        }

        public void AddPlugin(IPlugin plugin)
        {
            lock (Plugins)
            {
                Plugins.Add(plugin);
                plugin.OnStateChanged += Plugin_OnStateChanged;
            }
        }

        private void Plugin_OnStateChanged(IPlugin plugin, IPlugin.EventHandlerArgs<IPlugin> e)
        {
            PluginStatus status = plugin.Enabled ? PluginStatus.Enabled : PluginStatus.Disabled;

            EventBus.PublishEvent(new PluginState() { Id = plugin.Id, PluginName = plugin.Name, PluginStatus = status, DisplayName = plugin.DisplayName });
        }

        public void RemovePlugin(IPlugin plugin)
        {
            lock (Plugins)
            {
                plugin.OnStateChanged -= Plugin_OnStateChanged;
                Plugins.Remove(plugin);
            }
        }

        override protected void Main()
        {
            while (!Stopped)
            {
                int invoked = 0;

                IEvent? e;

                while ((e = Subscription?.NextEvent(10)) != null)
                {
                    lock (Plugins)
                    {
                        foreach (var plugin in Plugins)
                        {
                            if (plugin.PendingOnEnable)
                            {
                                plugin.OnEnable();
                                plugin.PendingOnEnable = false;
                            }

                            if (plugin.PendingOnDisable)
                            {
                                plugin.OnDisable();
                                plugin.PendingOnDisable = false;
                            }

                            plugin.EventHandler.HandleEvent(e);
                        }
                    }
                }


                lock (Plugins)
                {
                    foreach (var plugin in Plugins)
                    {
                        plugin.Loop();
                        invoked += 1;
                    }
                }

                if (invoked == 0)
                {
                    // Avoid busy-looping if there are no plugins
                    Thread.Sleep(10);
                }
            }
        }
    }
}
