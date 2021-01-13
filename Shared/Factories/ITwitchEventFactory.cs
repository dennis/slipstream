using Slipstream.Shared.Events.Twitch;

#nullable enable

namespace Slipstream.Shared.Factories
{
    public interface ITwitchEventFactory
    {
        TwitchCommandSendMessage CreateTwitchCommandSendMessage(string message);
        TwitchConnected CreateTwitchConnected();
        TwitchDisconnected CreateTwitchDisconnected();
        TwitchReceivedMessage CreateTwitchReceivedMessage(string from, string message, bool moderator, bool subscriber, bool vip, bool broadcaster);
        TwitchReceivedWhisper CreateTwitchReceivedWhisper(string from, string message);
        TwitchCommandSendWhisper CreateTwitchCommandSendWhisper(string to, string message);
    }
}
