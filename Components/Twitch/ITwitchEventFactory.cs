#nullable enable

using Slipstream.Components.Twitch.Events;
using Slipstream.Shared;

namespace Slipstream.Components.Twitch
{
    public interface ITwitchEventFactory
    {
        TwitchCommandSendMessage CreateTwitchCommandSendMessage(IEventEnvelope envelope, string message);

        TwitchConnected CreateTwitchConnected(IEventEnvelope envelope);

        TwitchDisconnected CreateTwitchDisconnected(IEventEnvelope envelope);

        TwitchReceivedMessage CreateTwitchReceivedMessage(IEventEnvelope envelope, string from, string message, bool moderator, bool subscriber, bool vip, bool broadcaster);

        TwitchReceivedWhisper CreateTwitchReceivedWhisper(IEventEnvelope envelope, string from, string message);

        TwitchCommandSendWhisper CreateTwitchCommandSendWhisper(IEventEnvelope envelope, string to, string message);

        TwitchUserSubscribed CreateTwitchUserSubscribed(IEventEnvelope envelope, string name, string message, string subscriptionPlan, long cumulativeMonths, long streakMonths, string systemMessage);

        TwitchGiftedSubscription CreateTwitchGiftedSubscription(IEventEnvelope envelope, string gifter, string subscriptionPlan, string recipient, string systemMessage);

        TwitchRaided CreateTwitchRaided(IEventEnvelope envelope, string name, int viewerCount);
    }
}