using Slipstream.Shared;

namespace Slipstream.Components.Discord.Events
{
    public class DiscordCommandSendMessage : IEvent
    {
        public string EventType => nameof(DiscordCommandSendMessage);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public ulong ChannelId { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool TextToSpeech { get; set; }
    }
}