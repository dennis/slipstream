﻿using Slipstream.Shared;
using System.Collections.Generic;
using System.Diagnostics;

namespace Slipstream.Backend
{
    public class EventBus : IEventBus
    {
        private readonly IList<EventBusSubscription> Listeners = new List<EventBusSubscription>();
        private readonly IList<IEvent> Events = new List<IEvent>(EVENT_MAX_SIZE);
        private readonly List<IEvent> PendingEvents = new List<IEvent>();
        private const int EVENT_MAX_SIZE = 1000;
        private const int EVENT_DELETE_SIZE = 200; // when we hit EVENT_MAX_SIZE. How many elements should we remove?
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
            e.Envelope.Uptime = Uptime() - StartedAt;

            lock (Listeners)
            {
                if (enabled)
                {
                    lock (Events)
                    {
                        if (Events.Count >= EVENT_MAX_SIZE)
                        {
                            // Is there a better way for deleting x elements from the beginning?
                            for (int i = 0; i < EVENT_DELETE_SIZE; i++)
                                Events.RemoveAt(0);
                        }
                        Events.Add(e);
                    }

                    foreach (var l in Listeners)
                    {
                        bool applicable = false;

                        foreach (var instanceId in l.InstanceIds)
                        {
                            if (e.Envelope.ContainsRecipient(instanceId))
                            {
                                applicable = true;
                                break;
                            }
                        }

                        if (applicable)
                        {
                            l.Add(e);
                        }
                    }
                }
                else
                {
                    PendingEvents.Add(e);
                }
            }
        }

        public IEventBusSubscription RegisterListener(string instanceId, bool fromStart = false)
        {
            var subscription = new EventBusSubscription(this, instanceId);

            lock (Listeners)
            {
                if (fromStart)
                {
                    lock (Events)
                    {
                        foreach (var e in Events)
                        {
                            if (e.Envelope.ContainsRecipient(instanceId))
                            {
                                subscription.Add(e);
                            }
                        }
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
                    PublishEvent(e);
                }

                PendingEvents.Clear();
            }
        }

        private ulong Uptime()
        {
            var tick = (double)Stopwatch.GetTimestamp() * 1000 / Stopwatch.Frequency;
            return (ulong)tick;
        }
    }
}