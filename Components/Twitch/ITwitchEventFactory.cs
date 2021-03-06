﻿#nullable enable

using Slipstream.Components.Twitch.Events;

namespace Slipstream.Components.Twitch
{
    public interface ITwitchEventFactory
    {
        TwitchCommandSendMessage CreateTwitchCommandSendMessage(string message);

        TwitchConnected CreateTwitchConnected();

        TwitchDisconnected CreateTwitchDisconnected();

        TwitchReceivedMessage CreateTwitchReceivedMessage(string from, string message, bool moderator, bool subscriber, bool vip, bool broadcaster);

        TwitchReceivedWhisper CreateTwitchReceivedWhisper(string from, string message);

        TwitchCommandSendWhisper CreateTwitchCommandSendWhisper(string to, string message);

        TwitchUserSubscribed CreateTwitchUserSubscribed(string name, string message, string subscriptionPlan, long cumulativeMonths, long streakMonths, string systemMessage);

        TwitchGiftedSubscription CreateTwitchGiftedSubscription(string gifter, string subscriptionPlan, string recipient, string systemMessage);

        TwitchRaided CreateTwitchRaided(string name, int viewerCount);
    }
}