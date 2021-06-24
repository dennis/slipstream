using Slipstream.Components.Discord.Events;
using Slipstream.Shared;

namespace Slipstream.Components.Discord.EventFactory
{
    internal class DiscordEventFactory : IDiscordEventFactory
    {
        public DiscordCommandSendMessage CreateDiscordCommandSendMessage(IEventEnvelope envelope, ulong channelId, string message, bool tts)
        {
            return new DiscordCommandSendMessage
            {
                Envelope = envelope,
                ChannelId = channelId,
                Message = message,
                TextToSpeech = tts
            };
        }

        public DiscordConnected CreateDiscordConnected(IEventEnvelope envelope)
        {
            return new DiscordConnected
            {
                Envelope = envelope,
            };
        }

        public DiscordDisconnected CreateDiscordDisconnected(IEventEnvelope envelope)
        {
            return new DiscordDisconnected
            {
                Envelope = envelope,
            };
        }

        public DiscordMessageReceived CreateDiscordMessageReceived(IEventEnvelope envelope, ulong fromId, string from, ulong channelId, string channel, string message)
        {
            return new DiscordMessageReceived
            {
                Envelope = envelope,
                FromId = fromId,
                From = from,
                ChannelId = channelId,
                Channel = channel,
                Message = message
            };
        }
    }
}