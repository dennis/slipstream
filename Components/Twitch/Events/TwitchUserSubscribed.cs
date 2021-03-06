﻿using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.Twitch.Events
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
        public long CumulativeMonths { get; set; }
        public long StreakMonths { get; set; }

        public override bool Equals(object obj)
        {
            return obj is TwitchUserSubscribed subscribed &&
                   EventType == subscribed.EventType &&
                   ExcludeFromTxrx == subscribed.ExcludeFromTxrx &&
                   Name == subscribed.Name &&
                   Message == subscribed.Message &&
                   SystemMessage == subscribed.SystemMessage &&
                   SubscriptionPlan == subscribed.SubscriptionPlan &&
                   CumulativeMonths == subscribed.CumulativeMonths &&
                   StreakMonths == subscribed.StreakMonths;
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
            hashCode = hashCode * -1521134295 + CumulativeMonths.GetHashCode();
            hashCode = hashCode * -1521134295 + StreakMonths.GetHashCode();
            return hashCode;
        }
    }
}