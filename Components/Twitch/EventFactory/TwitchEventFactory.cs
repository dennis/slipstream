using Slipstream.Components.Twitch.Events;

#nullable enable

namespace Slipstream.Components.Twitch.EventFactory
{
    public class TwitchEventFactory : ITwitchEventFactory
    {
        public TwitchCommandSendMessage CreateTwitchCommandSendMessage(string instanceId, string message)
        {
            return new TwitchCommandSendMessage
            {
                InstanceId = instanceId,
                Message = message
            };
        }

        public TwitchConnected CreateTwitchConnected(string instanceId)
        {
            return new TwitchConnected { InstanceId = instanceId };
        }

        public TwitchDisconnected CreateTwitchDisconnected(string instanceId)
        {
            return new TwitchDisconnected { InstanceId = instanceId };
        }

        public TwitchReceivedMessage CreateTwitchReceivedMessage(string instanceId, string from, string message, bool moderator, bool subscriber, bool vip, bool broadcaster)
        {
            return new TwitchReceivedMessage
            {
                InstanceId = instanceId,
                From = from,
                Message = message,
                Moderator = moderator,
                Subscriber = subscriber,
                Vip = vip,
                Broadcaster = broadcaster
            };
        }

        public TwitchReceivedWhisper CreateTwitchReceivedWhisper(string instanceId, string from, string message)
        {
            return new TwitchReceivedWhisper
            {
                InstanceId = instanceId,
                From = from,
                Message = message
            };
        }

        public TwitchCommandSendWhisper CreateTwitchCommandSendWhisper(string instanceId, string to, string message)
        {
            return new TwitchCommandSendWhisper
            {
                InstanceId = instanceId,
                To = to,
                Message = message
            };
        }

        public TwitchUserSubscribed CreateTwitchUserSubscribed(string instanceId, string name, string message, string subscriptionPlan, long cumulativeMonths, long streakMonths, string systemMessage)
        {
            return new TwitchUserSubscribed
            {
                InstanceId = instanceId,
                Name = name,
                Message = message,
                SystemMessage = systemMessage,
                SubscriptionPlan = subscriptionPlan,
                CumulativeMonths = cumulativeMonths,
                StreakMonths = streakMonths,
            };
        }

        public TwitchGiftedSubscription CreateTwitchGiftedSubscription(string instanceId, string gifter, string subscriptionPlan, string recipient, string systemMessage)
        {
            return new TwitchGiftedSubscription
            {
                InstanceId = instanceId,
                Gifter = gifter,
                SubscriptionPlan = subscriptionPlan,
                Recipient = recipient,
                SystemMessage = systemMessage,
            };
        }

        public TwitchRaided CreateTwitchRaided(string instanceId, string name, int viewerCount)
        {
            return new TwitchRaided
            {
                InstanceId = instanceId,
                Name = name,
                ViewerCount = viewerCount,
            };
        }
    }
}