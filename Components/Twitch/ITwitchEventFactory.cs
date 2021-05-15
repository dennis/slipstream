#nullable enable

using Slipstream.Components.Twitch.Events;

namespace Slipstream.Components.Twitch
{
    public interface ITwitchEventFactory
    {
        TwitchCommandSendMessage CreateTwitchCommandSendMessage(string instanceId, string message);

        TwitchConnected CreateTwitchConnected(string instanceId);

        TwitchDisconnected CreateTwitchDisconnected(string instanceId);

        TwitchReceivedMessage CreateTwitchReceivedMessage(string instanceId, string from, string message, bool moderator, bool subscriber, bool vip, bool broadcaster);

        TwitchReceivedWhisper CreateTwitchReceivedWhisper(string instanceId, string from, string message);

        TwitchCommandSendWhisper CreateTwitchCommandSendWhisper(string instanceId, string to, string message);

        TwitchUserSubscribed CreateTwitchUserSubscribed(string instanceId, string name, string message, string subscriptionPlan, long cumulativeMonths, long streakMonths, string systemMessage);

        TwitchGiftedSubscription CreateTwitchGiftedSubscription(string instanceId, string gifter, string subscriptionPlan, string recipient, string systemMessage);

        TwitchRaided CreateTwitchRaided(string instanceId, string name, int viewerCount);
    }
}