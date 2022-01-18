using Slipstream.Components.Discord.Events;
using Slipstream.Shared;

namespace Slipstream.Components.Discord
{
    public interface IDiscordEventFactory
    {
        DiscordCommandSendMessage CreateDiscordCommandSendMessage(IEventEnvelope envelope, ulong channelId, string message, bool tts);

        DiscordConnected CreateDiscordConnected(IEventEnvelope envelope);

        DiscordDisconnected CreateDiscordDisconnected(IEventEnvelope envelope);

        DiscordMessageReceived CreateDiscordMessageReceived(IEventEnvelope envelope, ulong fromId, string from, ulong channelId, string channel, string message);
    }
}