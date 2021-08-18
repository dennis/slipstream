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
        private readonly bool PromiscuousMode = false;
        private List<string> InstanceIds { get; set; } = new List<string>();

        public EventBusSubscription(IEventBus eventBus, string instanceId, bool promiscuousMode = false)
        {
            EventBus = eventBus;
            InstanceIds.Add(instanceId);
            PromiscuousMode = promiscuousMode;
        }

        public void Add(IEvent ev)
        {
            if (PromiscuousMode)
            {
                Events.Add(ev);
            }
            else
            {
                foreach (var instanceId in InstanceIds)
                {
                    if (ev.Envelope.ContainsRecipient(instanceId))
                    {
                        Events.Add(ev);
                        return;
                    }
                }
            }
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
            if (Events.TryTake(out IEvent? ev, millisecondsTimeout))
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