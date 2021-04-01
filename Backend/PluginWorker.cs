using Slipstream.Components;
using Slipstream.Components.Internal;
using Slipstream.Shared;
using System.Diagnostics;
using static Slipstream.Components.Internal.IInternalEventFactory;

#nullable enable

namespace Slipstream.Backend
{
    internal class PluginWorker : Worker
    {
        public IPlugin Plugin { get; }
        private readonly IEventBusSubscription Subscription;
        private readonly IEventBus EventBus;
        private readonly IInternalEventFactory EventFactory;

        public PluginWorker(IPlugin plugin, IEventBusSubscription subscription, IInternalEventFactory eventFactory, IEventBus eventBus) : base(plugin.Id)
        {
            Plugin = plugin;
            plugin.OnStateChanged += Plugin_OnStateChanged;
            Subscription = subscription;
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        private void Plugin_OnStateChanged(object source, IPlugin plugin)
        {
            EventBus.PublishEvent(EventFactory.CreateInternalPluginState(plugin.Id, plugin.Name, plugin.DisplayName, PluginStatusEnum.Registered));
        }

        override protected void Main()
        {
            if (Plugin.FullThreadControl)
            {
                // We dont need the subscription
                Subscription?.Dispose();
                Plugin.Run();
            }
            else
            {
                WePerformMainLoop();
            }
        }

        private void WePerformMainLoop()
        {
            while (!Stopped)
            {
                IEvent? e;

                var tick = GetTick(); // only do work for 0.1s, before allowing Plugin.Loop() to get invoked
                while ((e = Subscription?.NextEvent(10)) != null)
                {
                    Plugin.EventHandlerController.HandleEvent(e);

                    if (GetTick() - tick > 100)
                        break;
                }

                Plugin.Run();
            }

            Subscription?.Dispose();
        }

        private long GetTick()
        {
            return (long)((double)Stopwatch.GetTimestamp() * 1000 / Stopwatch.Frequency);
        }
    }
}