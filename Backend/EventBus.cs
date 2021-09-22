using Slipstream.Shared;

using System.Collections.Generic;
using System.Diagnostics;

namespace Slipstream.Backend
{
    public class EventBus : IEventBus
    {
        private readonly IList<EventBusSubscription> Listeners = new List<EventBusSubscription>();
        private readonly ICircularBlockingQueue<IEvent> Events = new CircularBlockingQueue<IEvent>(EVENT_MAX_SIZE);
        private readonly List<IEvent> PendingEvents = new List<IEvent>();
        private const int EVENT_MAX_SIZE = 1000;
        private volatile bool enabled = false;
        private readonly ulong StartedAt;

        public EventBus()
        {
            StartedAt = Uptime();
        }

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
                e.Envelope.Uptime = Uptime() - StartedAt;

                if (enabled)
                {
                    Events.Enqueue(e);
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

        public IEventBusSubscription RegisterListener(string instanceId, bool fromStart = false, bool promiscuousMode = false)
        {
            var subscription = new EventBusSubscription(this, instanceId, promiscuousMode);

            lock (Listeners)
            {
                if (fromStart)
                {
                    // As this is a queue, we need to clone it before consuming it. We can't use Events directory,
                    // as this would clear any existing events from it.
                    var cloned = Events.Clone();

                    IEvent? e;
                    while ((e = cloned.Dequeue(0)) != null)
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
                foreach (var e in PendingEvents)
                {
                    Events.Enqueue(e);
                    foreach (var l in Listeners)
                    {
                        l.Add(e);
                    }
                }

                PendingEvents.Clear();
                enabled = true;
            }
        }

        private static ulong Uptime()
        {
            var tick = (double)Stopwatch.GetTimestamp() * 1000 / Stopwatch.Frequency;
            return (ulong)tick;
        }
    }
}