using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.Twitch.Events
{
    public class TwitchGiftedSubscription : IEvent
    {
        public string EventType => "TwitchGiftedSubscription";
        public ulong Uptime { get; set; }
        public string InstanceId { get; set; } = string.Empty;
        public string Gifter { get; set; } = string.Empty;
        public string SubscriptionPlan { get; set; } = string.Empty;
        public string Recipient { get; set; } = string.Empty;
        public string SystemMessage { get; set; } = string.Empty;

        public override bool Equals(object obj)
        {
            return obj is TwitchGiftedSubscription subscription &&
                   EventType == subscription.EventType &&
                   InstanceId == subscription.InstanceId &&
                   Gifter == subscription.Gifter &&
                   SubscriptionPlan == subscription.SubscriptionPlan &&
                   Recipient == subscription.Recipient &&
                   SystemMessage == subscription.SystemMessage;
        }

        public override int GetHashCode()
        {
            int hashCode = -608133484;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(InstanceId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Gifter);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SubscriptionPlan);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Recipient);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SystemMessage);
            return hashCode;
        }
    }
}