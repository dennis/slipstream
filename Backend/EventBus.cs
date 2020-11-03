using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Backend
{
    public class EventBus : IEventBus
    {
        // TODO - use WeakReference  ?
        private readonly IDictionary<string, EventBusSubscription> Listeners = new Dictionary<string, EventBusSubscription>();

        public void PublishEvent(IEvent e)
        {
            lock (Listeners)
            {
                foreach (var l in Listeners)
                {
                    l.Value.Add(e);
                }
            }
        }

        public IEventBusSubscription RegisterListener(IEventListener listener)
        {
            return RegisterListener(listener.Name);
        }

        public IEventBusSubscription RegisterListener(string listenerName)
        {
            var subscription = new EventBusSubscription();

            lock (Listeners)
            {
                Listeners.Add(listenerName, subscription);
            }

            return subscription;
        }

        public void UnregisterListener(IEventListener listener)
        {
            UnregisterListener(listener.Name);
        }

        public void UnregisterListener(string listenerName)
        {
            lock (Listeners)
            {
                Listeners.Remove(listenerName);
            }
        }

    }
}
