using Slipstream.Shared;
using System.Collections.Concurrent;

#nullable enable

namespace Slipstream.Backend
{
    internal class EventBusSubscription : IEventBusSubscription
    {
        private readonly BlockingCollection<IEvent> Events = new BlockingCollection<IEvent>();
        private readonly IEventBus EventBus;
        public readonly string InstanceId;

        public EventBusSubscription(IEventBus eventBus, string instanceId)
        {
            EventBus = eventBus;
            InstanceId = instanceId;
        }

        public void Add(IEvent ev)
        {
            Events.Add(ev);
        }

        public void Dispose()
        {
            EventBus.UnregisterSubscription(this);
        }

        public IEvent NextEvent()
        {
            return Events.Take();
        }

        public IEvent? NextEvent(int millisecondsTimeout)
        {
            if (Events.TryTake(out IEvent ev, millisecondsTimeout))
            {
                return ev;
            }

            return null;
        }
    }
}