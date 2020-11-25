using Slipstream.Shared;
using System.Collections.Generic;
using System.Diagnostics;

namespace Slipstream.Backend
{
    public class EventBus : IEventBus
    {
        private readonly IList<EventBusSubscription> Listeners = new List<EventBusSubscription>();

        public void PublishEvent(IEvent e)
        {
            Debug.WriteLine($"PublishEvent {e}");

            lock (Listeners)
            {
                foreach (var l in Listeners)
                {
                    l.Add(e);
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
    }
}
