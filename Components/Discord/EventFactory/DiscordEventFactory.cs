using Slipstream.Components.Discord.Events;

namespace Slipstream.Components.Discord.EventFactory
{
    internal class DiscordEventFactory : IDiscordEventFactory
    {
        public DiscordCommandSendMessage CreateDiscordCommandSendMessage(ulong channelId, string message, bool tts)
        {
            return new DiscordCommandSendMessage
            {
                ChannelId = channelId,
                Message = message,
                TextToSpeech = tts
            };
        }

        public DiscordConnected CreateDiscordConnected()
        {
            return new DiscordConnected();
        }

        public DiscordDisconnected CreateDiscordDisconnected()
        {
            return new DiscordDisconnected();
        }

        public DiscordMessageReceived CreateDiscordMessageReceived(ulong fromId, string from, ulong channelId, string channel, string message)
        {
            return new DiscordMessageReceived
            {
                FromId = fromId,
                From = from,
                ChannelId = channelId,
                Channel = channel,
                Message = message
            };
        }
    }
}