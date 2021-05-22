using Slipstream.Components.Discord.Events;

namespace Slipstream.Components.Discord
{
    public interface IDiscordEventFactory
    {
        DiscordCommandSendMessage CreateDiscordCommandSendMessage(string instanceId, ulong channelId, string message, bool tts);

        DiscordConnected CreateDiscordConnected(string instanceId);

        DiscordDisconnected CreateDiscordDisconnected(string instanceId);

        DiscordMessageReceived CreateDiscordMessageReceived(string instanceId, ulong fromId, string from, ulong channelId, string channel, string message);
    }
}