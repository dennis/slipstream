using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Backend
{
    public class EventBus : IEventBus
    {
        private readonly IList<EventBusSubscription> Listeners = new List<EventBusSubscription>();

        public void PublishEvent(IEvent e)
        {
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
                Listeners.Remove(subscription as EventBusSubscription);
            }
        }
    }
}
