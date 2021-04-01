#nullable enable

using Slipstream.Components.Twitch.Events;
using Slipstream.Shared;
using System;

namespace Slipstream.Components.Twitch.EventHandler
{
    internal class Twitch : IEventHandler
    {
        public event EventHandler<TwitchCommandSendMessage>? OnTwitchCommandSendMessage;

        public event EventHandler<TwitchCommandSendWhisper>? OnTwitchCommandSendWhisper;

        public event EventHandler<TwitchConnected>? OnTwitchConnected;

        public event EventHandler<TwitchDisconnected>? OnTwitchDisconnected;

        public event EventHandler<TwitchReceivedMessage>? OnTwitchReceivedMessage;

        public event EventHandler<TwitchReceivedWhisper>? OnTwitchReceivedWhisper;

        public event EventHandler<TwitchUserSubscribed>? OnTwitchUserSubscribed;

        public event EventHandler<TwitchGiftedSubscription>? OnTwitchGiftedSubscription;

        public event EventHandler<TwitchRaided>? OnTwitchRaided;

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            return @event switch
            {
                TwitchConnected tev => OnEvent(OnTwitchConnected, tev),
                TwitchDisconnected tev => OnEvent(OnTwitchDisconnected, tev),
                TwitchCommandSendMessage tev => OnEvent(OnTwitchCommandSendMessage, tev),
                TwitchCommandSendWhisper tev => OnEvent(OnTwitchCommandSendWhisper, tev),
                TwitchReceivedMessage tev => OnEvent(OnTwitchReceivedMessage, tev),
                TwitchReceivedWhisper tev => OnEvent(OnTwitchReceivedWhisper, tev),
                TwitchUserSubscribed tev => OnEvent(OnTwitchUserSubscribed, tev),
                TwitchGiftedSubscription tev => OnEvent(OnTwitchGiftedSubscription, tev),
                TwitchRaided tev => OnEvent(OnTwitchRaided, tev),
                _ => IEventHandler.HandledStatus.NotMine,
            };
        }

        private IEventHandler.HandledStatus OnEvent<TEvent>(EventHandler<TEvent>? onEvent, TEvent args)
        {
            if (onEvent != null)
            {
                onEvent.Invoke(this, args);
                return IEventHandler.HandledStatus.Handled;
            }
            return IEventHandler.HandledStatus.UseDefault;
        }
    }
}