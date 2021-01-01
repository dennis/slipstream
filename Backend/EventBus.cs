using Slipstream.Shared;
using System.Collections.Generic;
using System.Diagnostics;

namespace Slipstream.Backend
{
    public class EventBus : IEventBus
    {
        private readonly IList<EventBusSubscription> Listeners = new List<EventBusSubscription>();
        private readonly IList<IEvent> Events = new List<IEvent>(EVENT_MAX_SIZE);
        private readonly List<IEvent> PendingEvents = new List<IEvent>();
        private const int EVENT_MAX_SIZE = 10000;
        private const int EVENT_DELETE_SIZE = 1000; // when we hit EVENT_MAX_SIZE. How many elements should we remove?
        private volatile bool enabled = false;

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                Debug.Assert(value && !enabled);

                EnableAndFlushPendingEvents();
            }
        }
        public void PublishEvent(IEvent e)
        {
            lock (Events)
            {
                if(Events.Count >= EVENT_MAX_SIZE)
                {
                    // Is there a better way for deleting x elements from the beginning?
                    for (int i = 0; i < EVENT_DELETE_SIZE; i++)
                        Events.RemoveAt(0);
                }
                Events.Add(e);
            }

            lock (Listeners)
            {
                if (enabled)
                {
                    foreach (var l in Listeners)
                    {
                        l.Add(e);
                    }
                }
                else
                {
                    PendingEvents.Add(e);
                }
            }
        }

        public IEventBusSubscription RegisterListener(bool fromStart = false)
        {
            var subscription = new EventBusSubscription(this);

            lock (Listeners)
            {
                if (fromStart)
                {
                    foreach(var e in Events)
                    {
                        subscription.Add(e);
                    }
                }

                Listeners.Add(subscription);
            }

            return subscription;
        }

        public void UnregisterSubscription(IEventBusSubscription subscription)
        {
            lock (Listeners)
            {
                if (subscription is EventBusSubscription a)
                    _ = Listeners.Remove(a);
            }
        }

        private void EnableAndFlushPendingEvents()
        {
            lock (Listeners)
            {
                enabled = true;
                foreach (var e in PendingEvents)
                {
                    foreach (var l in Listeners)
                    {
                        l.Add(e);
                    }
                }

                PendingEvents.Clear();
            }
        }
    }
}
