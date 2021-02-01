#nullable enable

using Slipstream.Components.Twitch.Events;
using Slipstream.Shared;

namespace Slipstream.Components.Twitch.EventHandler
{
    internal class Twitch : IEventHandler
    {
        private readonly EventHandlerController Parent;

        public Twitch(EventHandlerController eventHandler)
        {
            Parent = eventHandler;
        }

        public delegate void OnTwitchCommandSendMessageHandler(EventHandlerController source, EventHandlerArgs<TwitchCommandSendMessage> e);

        public delegate void OnTwitchCommandSendWhisperHandler(EventHandlerController source, EventHandlerArgs<TwitchCommandSendWhisper> e);

        public delegate void OnTwitchConnectedHandler(EventHandlerController source, EventHandlerArgs<TwitchConnected> e);

        public delegate void OnTwitchDisconnectedHandler(EventHandlerController source, EventHandlerArgs<TwitchDisconnected> e);

        public delegate void OnTwitchReceivedMessageHandler(EventHandlerController source, EventHandlerArgs<TwitchReceivedMessage> e);

        public delegate void OnTwitchReceivedWhisperHandler(EventHandlerController source, EventHandlerArgs<TwitchReceivedWhisper> e);

        public delegate void OnTwitchUserSubscribedHandler(EventHandlerController source, EventHandlerArgs<TwitchUserSubscribed> e);

        public delegate void OnTwitchGiftedSubscriptionHandler(EventHandlerController source, EventHandlerArgs<TwitchGiftedSubscription> e);

        public delegate void OnTwitchRaidedHandler(EventHandlerController source, EventHandlerArgs<TwitchRaided> e);

        public event OnTwitchCommandSendMessageHandler? OnTwitchCommandSendMessage;

        public event OnTwitchCommandSendWhisperHandler? OnTwitchCommandSendWhisper;

        public event OnTwitchConnectedHandler? OnTwitchConnected;

        public event OnTwitchDisconnectedHandler? OnTwitchDisconnected;

        public event OnTwitchReceivedMessageHandler? OnTwitchReceivedMessage;

        public event OnTwitchReceivedWhisperHandler? OnTwitchReceivedWhisper;

        public event OnTwitchUserSubscribedHandler? OnTwitchUserSubscribed;

        public event OnTwitchGiftedSubscriptionHandler? OnTwitchGiftedSubscription;

        public event OnTwitchRaidedHandler? OnTwitchRaided;

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
                case TwitchRaided tev:
                    if (OnTwitchRaided != null)
                    {
                        OnTwitchRaided.Invoke(Parent, new EventHandlerArgs<TwitchRaided>(tev));
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