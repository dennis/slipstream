using Slipstream.Shared.Events.Twitch;

#nullable enable

namespace Slipstream.Shared.Factories
{
    public class TwitchEventFactory : ITwitchEventFactory
    {
        public TwitchCommandSendMessage CreateTwitchCommandSendMessage(string message)
        {
            return new TwitchCommandSendMessage
            {
                Message = message
            };
        }

        public TwitchConnected CreateTwitchConnected()
        {
            return new TwitchConnected();
        }

        public TwitchDisconnected CreateTwitchDisconnected()
        {
            return new TwitchDisconnected();
        }

        public TwitchReceivedMessage CreateTwitchReceivedMessage(string from, string message, bool moderator, bool subscriber, bool vip, bool broadcaster)
        {
            return new TwitchReceivedMessage
            {
                From = from,
                Message = message,
                Moderator = moderator,
                Subscriber = subscriber,
                Vip = vip,
                Broadcaster = broadcaster
            };
        }

        public TwitchReceivedWhisper CreateTwitchReceivedWhisper(string from, string message)
        {
            return new TwitchReceivedWhisper
            {
                From = from,
                Message = message
            };
        }

        public TwitchCommandSendWhisper CreateTwitchCommandSendWhisper(string to, string message)
        {
            return new TwitchCommandSendWhisper
            {
                To = to,
                Message = message
            };
        }
    }
}
