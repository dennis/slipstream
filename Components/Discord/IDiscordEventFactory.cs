using Slipstream.Components.Discord.Events;

namespace Slipstream.Components.Discord
{
    public interface IDiscordEventFactory
    {
        DiscordCommandSendMessage CreateDiscordCommandSendMessage(ulong channelId, string message, bool tts);

        DiscordConnected CreateDiscordConnected();

        DiscordDisconnected CreateDiscordDisconnected();

        DiscordMessageReceived CreateDiscordMessageReceived(ulong fromId, string from, ulong channelId, string channel, string message);
    }
}