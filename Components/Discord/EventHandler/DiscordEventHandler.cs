using Slipstream.Components.Discord.Events;
using Slipstream.Shared;

namespace Slipstream.Components.Discord.EventHandler
{
    internal class DiscordEventHandler : IEventHandler
    {
        private readonly EventHandlerController Parent;

        public delegate void OnDiscordConnectedHandler(EventHandlerController source, EventHandlerArgs<DiscordConnected> e);

        public delegate void OnDiscordDisconnectedHandler(EventHandlerController source, EventHandlerArgs<DiscordDisconnected> e);

        public delegate void OnDiscordMessageReceivedHandler(EventHandlerController source, EventHandlerArgs<DiscordMessageReceived> e);

        public delegate void OnDiscordCommandSendMessageHandler(EventHandlerController source, EventHandlerArgs<DiscordCommandSendMessage> e);

        public event OnDiscordConnectedHandler? OnDiscordConnected;

        public event OnDiscordDisconnectedHandler? OnDiscordDisconnected;

        public event OnDiscordMessageReceivedHandler? OnDiscordMessageReceived;

        public event OnDiscordCommandSendMessageHandler? OnDiscordCommandSendMessage;

        public DiscordEventHandler(EventHandlerController eventHandler)
        {
            Parent = eventHandler;
        }

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            switch (@event)
            {
                case DiscordConnected tev:
                    if (OnDiscordConnected != null)
                    {
                        OnDiscordConnected.Invoke(Parent, new EventHandlerArgs<DiscordConnected>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case DiscordDisconnected tev:
                    if (OnDiscordDisconnected != null)
                    {
                        OnDiscordDisconnected.Invoke(Parent, new EventHandlerArgs<DiscordDisconnected>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case DiscordMessageReceived tev:
                    if (OnDiscordMessageReceived != null)
                    {
                        OnDiscordMessageReceived.Invoke(Parent, new EventHandlerArgs<DiscordMessageReceived>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case DiscordCommandSendMessage tev:
                    if (OnDiscordCommandSendMessage != null)
                    {
                        OnDiscordCommandSendMessage.Invoke(Parent, new EventHandlerArgs<DiscordCommandSendMessage>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
            }

            return IEventHandler.HandledStatus.NotMine;
        }
    }
}