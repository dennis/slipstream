using Slipstream.Shared;

namespace Slipstream.Components.Discord.Events
{
    public class DiscordCommandSendMessage : IEvent
    {
        public string EventType => "DiscordCommandSendMessage";

        public bool ExcludeFromTxrx => false;

        public ulong Uptime { get; set; }

        public ulong ChannelId { get; set; }

        public string Message { get; set; } = string.Empty;

        public bool TextToSpeech { get; set; }
    }
}