using Slipstream.Shared;
using System.Collections.Concurrent;
using System.Collections.Generic;

#nullable enable

namespace Slipstream.Backend
{
    internal class EventBusSubscription : IEventBusSubscription
    {
        private readonly BlockingCollection<IEvent> Events = new BlockingCollection<IEvent>();
        private readonly IEventBus EventBus;
        public List<string> InstanceIds { get; set; } = new List<string>();

        public EventBusSubscription(IEventBus eventBus, string instanceId)
        {
            EventBus = eventBus;
            InstanceIds.Add(instanceId);
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

        public void AddImpersonate(string instanceId)
        {
            InstanceIds.Add(instanceId);
        }

        public void DeleteImpersonation(string instanceId)
        {
            InstanceIds.Remove(instanceId);
        }
    }
}