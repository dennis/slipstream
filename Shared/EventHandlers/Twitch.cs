#nullable enable

using Slipstream.Shared.Events.Twitch;
using static Slipstream.Shared.EventHandler;

namespace Slipstream.Shared.EventHandlers
{
    internal class Twitch : IEventHandler
    {
        private readonly EventHandler Parent;

        public Twitch(EventHandler eventHandler)
        {
            Parent = eventHandler;
        }

        public delegate void OnTwitchCommandSendMessageHandler(EventHandler source, EventHandlerArgs<TwitchCommandSendMessage> e);

        public delegate void OnTwitchCommandSendWhisperHandler(EventHandler source, EventHandlerArgs<TwitchCommandSendWhisper> e);

        public delegate void OnTwitchConnectedHandler(EventHandler source, EventHandlerArgs<TwitchConnected> e);

        public delegate void OnTwitchDisconnectedHandler(EventHandler source, EventHandlerArgs<TwitchDisconnected> e);

        public delegate void OnTwitchReceivedMessageHandler(EventHandler source, EventHandlerArgs<TwitchReceivedMessage> e);

        public delegate void OnTwitchReceivedWhisperHandler(EventHandler source, EventHandlerArgs<TwitchReceivedWhisper> e);

        public delegate void OnTwitchUserSubscribedHandler(EventHandler source, EventHandlerArgs<TwitchUserSubscribed> e);

        public delegate void OnTwitchGiftedSubscriptionHandler(EventHandler source, EventHandlerArgs<TwitchGiftedSubscription> e);

        public event OnTwitchCommandSendMessageHandler? OnTwitchCommandSendMessage;

        public event OnTwitchCommandSendWhisperHandler? OnTwitchCommandSendWhisper;

        public event OnTwitchConnectedHandler? OnTwitchConnected;

        public event OnTwitchDisconnectedHandler? OnTwitchDisconnected;

        public event OnTwitchReceivedMessageHandler? OnTwitchReceivedMessage;

        public event OnTwitchReceivedWhisperHandler? OnTwitchReceivedWhisper;

        public event OnTwitchUserSubscribedHandler? OnTwitchUserSubscribed;

        public event OnTwitchGiftedSubscriptionHandler? OnTwitchGiftedSubscription;

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            switch (@event)
            {
                case TwitchConnected tev:
                    if (OnTwitchConnected != null)
                    {
                        OnTwitchConnected.Invoke(Parent, new EventHandlerArgs<TwitchConnected>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case TwitchDisconnected tev:
                    if (OnTwitchDisconnected != null)
                    {
                        OnTwitchDisconnected.Invoke(Parent, new EventHandlerArgs<TwitchDisconnected>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case TwitchCommandSendMessage tev:
                    if (OnTwitchCommandSendMessage != null)
                    {
                        OnTwitchCommandSendMessage.Invoke(Parent, new EventHandlerArgs<TwitchCommandSendMessage>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case TwitchCommandSendWhisper tev:
                    if (OnTwitchCommandSendWhisper != null)
                    {
                        OnTwitchCommandSendWhisper.Invoke(Parent, new EventHandlerArgs<TwitchCommandSendWhisper>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case TwitchReceivedMessage tev:
                    if (OnTwitchReceivedMessage != null)
                    {
                        OnTwitchReceivedMessage.Invoke(Parent, new EventHandlerArgs<TwitchReceivedMessage>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case TwitchReceivedWhisper tev:
                    if (OnTwitchReceivedWhisper != null)
                    {
                        OnTwitchReceivedWhisper.Invoke(Parent, new EventHandlerArgs<TwitchReceivedWhisper>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case TwitchUserSubscribed tev:
                    if (OnTwitchUserSubscribed != null)
                    {
                        OnTwitchUserSubscribed.Invoke(Parent, new EventHandlerArgs<TwitchUserSubscribed>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case TwitchGiftedSubscription tev:
                    if (OnTwitchGiftedSubscription != null)
                    {
                        OnTwitchGiftedSubscription.Invoke(Parent, new EventHandlerArgs<TwitchGiftedSubscription>(tev));
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