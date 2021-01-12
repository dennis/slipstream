using Slipstream.Shared;
using Slipstream.Shared.Events.Internal;
using Slipstream.Shared.Factories;
using System.Collections.Generic;
using System.Threading;
using static Slipstream.Shared.Factories.IInternalEventFactory;

#nullable enable

namespace Slipstream.Backend
{
    internal class PluginWorker : Worker
    {
        private readonly IList<IPlugin> Plugins = new List<IPlugin>();
        private readonly IEventBusSubscription Subscription;
        private readonly IEventBus EventBus;
        private readonly IInternalEventFactory EventFactory;

        public PluginWorker(string name, IEventBusSubscription subscription, IInternalEventFactory eventFactory, IEventBus eventBus) : base(name)
        {
            Subscription = subscription;
            EventBus = eventBus;
            EventFactory = eventFactory;
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
            EventBus.PublishEvent(EventFactory.CreateInternalPluginState(plugin.Id, plugin.Name, plugin.DisplayName, PluginStatusEnum.Registered));
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
                            plugin.EventHandler.HandleEvent(e);
                        }
                    }
                }

                lock (Plugins)
                {
                    foreach (var plugin in Plugins)
                    {
                        plugin.Loop();
                        invoked++;
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
