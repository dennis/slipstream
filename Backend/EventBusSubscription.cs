using Serilog;

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
        private readonly ILogger Logger;
        private readonly bool PromiscuousMode = false;
        private List<string> InstanceIds { get; set; } = new List<string>();

        private static int IdMax = 0;

        public int Id { get; set; }
        public int Count { get => Events.Count; }

        public EventBusSubscription(IEventBus eventBus, string instanceId, ILogger logger, bool promiscuousMode = false)
        {
            Id = ++IdMax;
            EventBus = eventBus;
            InstanceIds.Add(instanceId);
            Logger = logger
                .ForContext("SlipstreamInstanceId", instanceId)
                .ForContext("SubscriptionId", Id)
                .ForContext(GetType());
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

        public IEvent? NextEvent(int millisecondsTimeout)
        {
            if (Events.TryTake(out IEvent? ev, millisecondsTimeout))
            {
                Logger.Debug("Subscription {SubscriptionId} received event: {EventType}@{Uptime} from {EventSender}. {EventCount} remaining events queued up", Id, ev.EventType, ev.Envelope.Uptime, ev.Envelope.Sender, Events.Count);
                return ev;
            }

            return null;
        }

        public void AddImpersonate(string instanceId)
        {
            Logger.Debug("Adding impersonation {InstanceId}", instanceId);
            InstanceIds.Add(instanceId);
        }

        public void DeleteImpersonation(string instanceId)
        {
            Logger.Debug("Deleting impersonation {InstanceId}", instanceId);
            InstanceIds.Remove(instanceId);
        }
    }
}