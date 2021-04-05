#nullable enable

using Slipstream.Components.Discord.Events;
using Slipstream.Shared;
using System;

namespace Slipstream.Components.Discord.EventHandler
{
    internal class DiscordEventHandler : IEventHandler
    {
        public event EventHandler<DiscordConnected>? OnDiscordConnected;

        public event EventHandler<DiscordDisconnected>? OnDiscordDisconnected;

        public event EventHandler<DiscordMessageReceived>? OnDiscordMessageReceived;

        public event EventHandler<DiscordCommandSendMessage>? OnDiscordCommandSendMessage;

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            return @event switch
            {
                DiscordConnected tev => OnEvent(OnDiscordConnected, tev),
                DiscordDisconnected tev => OnEvent(OnDiscordDisconnected, tev),
                DiscordMessageReceived tev => OnEvent(OnDiscordMessageReceived, tev),
                DiscordCommandSendMessage tev => OnEvent(OnDiscordCommandSendMessage, tev),
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