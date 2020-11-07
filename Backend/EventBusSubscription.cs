using Slipstream.Shared;
using System.Collections.Concurrent;

#nullable enable

namespace Slipstream.Backend
{
    class EventBusSubscription : IEventBusSubscription
    {
        private readonly BlockingCollection<IEvent> Events = new BlockingCollection<IEvent>();
        private readonly IEventBus EventBus;

        public EventBusSubscription(IEventBus eventBus)
        {
            EventBus = eventBus;
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
            else
            {
                return null;
            }
        }
    }
}