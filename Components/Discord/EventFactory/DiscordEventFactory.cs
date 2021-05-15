using Slipstream.Components.Discord.Events;

namespace Slipstream.Components.Discord.EventFactory
{
    internal class DiscordEventFactory : IDiscordEventFactory
    {
        public DiscordCommandSendMessage CreateDiscordCommandSendMessage(string instanceId, ulong channelId, string message, bool tts)
        {
            return new DiscordCommandSendMessage
            {
                InstanceId = instanceId,
                ChannelId = channelId,
                Message = message,
                TextToSpeech = tts
            };
        }

        public DiscordConnected CreateDiscordConnected(string instanceId)
        {
            return new DiscordConnected
            {
                InstanceId = instanceId
            };
        }

        public DiscordDisconnected CreateDiscordDisconnected(string instanceId)
        {
            return new DiscordDisconnected
            {
                InstanceId = instanceId,
            };
        }

        public DiscordMessageReceived CreateDiscordMessageReceived(string instanceId, ulong fromId, string from, ulong channelId, string channel, string message)
        {
            return new DiscordMessageReceived
            {
                InstanceId = instanceId,
                FromId = fromId,
                From = from,
                ChannelId = channelId,
                Channel = channel,
                Message = message
            };
        }
    }
}