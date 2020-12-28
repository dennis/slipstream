using Slipstream.Shared;
using System.Collections.Generic;
using System.Diagnostics;

namespace Slipstream.Backend
{
    public class EventBus : IEventBus
    {
        private readonly IList<EventBusSubscription> Listeners = new List<EventBusSubscription>();
        private readonly List<IEvent> PendingEvents = new List<IEvent>();
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

        public IEventBusSubscription RegisterListener()
        {
            var subscription = new EventBusSubscription(this);

            lock (Listeners)
            {
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
