using Slipstream.Components.Twitch.Events;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.Twitch.EventFactory
{
    public class TwitchEventFactory : ITwitchEventFactory
    {
        public TwitchCommandSendMessage CreateTwitchCommandSendMessage(IEventEnvelope envelope, string message)
        {
            return new TwitchCommandSendMessage
            {
                Envelope = envelope,
                Message = message
            };
        }

        public TwitchConnected CreateTwitchConnected(IEventEnvelope envelope)
        {
            return new TwitchConnected { Envelope = envelope };
        }

        public TwitchDisconnected CreateTwitchDisconnected(IEventEnvelope envelope)
        {
            return new TwitchDisconnected { Envelope = envelope };
        }

        public TwitchReceivedMessage CreateTwitchReceivedMessage(IEventEnvelope envelope, string from, string message, bool moderator, bool subscriber, bool vip, bool broadcaster)
        {
            return new TwitchReceivedMessage
            {
                Envelope = envelope,
                From = from,
                Message = message,
                Moderator = moderator,
                Subscriber = subscriber,
                Vip = vip,
                Broadcaster = broadcaster
            };
        }

        public TwitchReceivedWhisper CreateTwitchReceivedWhisper(IEventEnvelope envelope, string from, string message)
        {
            return new TwitchReceivedWhisper
            {
                Envelope = envelope,
                From = from,
                Message = message
            };
        }

        public TwitchCommandSendWhisper CreateTwitchCommandSendWhisper(IEventEnvelope envelope, string to, string message)
        {
            return new TwitchCommandSendWhisper
            {
                Envelope = envelope,
                To = to,
                Message = message
            };
        }

        public TwitchUserSubscribed CreateTwitchUserSubscribed(IEventEnvelope envelope, string name, string message, string subscriptionPlan, long cumulativeMonths, long streakMonths, string systemMessage)
        {
            return new TwitchUserSubscribed
            {
                Envelope = envelope,
                Name = name,
                Message = message,
                SystemMessage = systemMessage,
                SubscriptionPlan = subscriptionPlan,
                CumulativeMonths = cumulativeMonths,
                StreakMonths = streakMonths,
            };
        }

        public TwitchGiftedSubscription CreateTwitchGiftedSubscription(IEventEnvelope envelope, string gifter, string subscriptionPlan, string recipient, string systemMessage)
        {
            return new TwitchGiftedSubscription
            {
                Envelope = envelope,
                Gifter = gifter,
                SubscriptionPlan = subscriptionPlan,
                Recipient = recipient,
                SystemMessage = systemMessage,
            };
        }

        public TwitchRaided CreateTwitchRaided(IEventEnvelope envelope, string name, int viewerCount)
        {
            return new TwitchRaided
            {
                Envelope = envelope,
                Name = name,
                ViewerCount = viewerCount,
            };
        }
    }
}