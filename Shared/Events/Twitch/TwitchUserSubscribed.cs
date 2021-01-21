using System.Collections.Generic;

namespace Slipstream.Shared.Events.Twitch
{
    public class TwitchUserSubscribed : IEvent
    {
        public string EventType => "TwitchUserSubscribed";
        public bool ExcludeFromTxrx => false;
        public ulong Uptime { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string SystemMessage { get; set; } = string.Empty;
        public string SubscriptionPlan { get; set; } = string.Empty;
        public long Months { get; set; }

        public override bool Equals(object obj)
        {
            return obj is TwitchUserSubscribed subscribed &&
                   EventType == subscribed.EventType &&
                   ExcludeFromTxrx == subscribed.ExcludeFromTxrx &&
                   Name == subscribed.Name &&
                   Message == subscribed.Message &&
                   SystemMessage == subscribed.SystemMessage &&
                   SubscriptionPlan == subscribed.SubscriptionPlan &&
                   Months == subscribed.Months;
        }

        public override int GetHashCode()
        {
            int hashCode = -1258448020;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Message);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SystemMessage);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SubscriptionPlan);
            hashCode = hashCode * -1521134295 + Months.GetHashCode();
            return hashCode;
        }
    }
}