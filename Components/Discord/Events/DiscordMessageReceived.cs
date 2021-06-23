using Slipstream.Shared;

namespace Slipstream.Components.Discord.Events
{
    public class DiscordMessageReceived : IEvent
    {
        public string EventType => nameof(DiscordMessageReceived);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public string From { get; set; } = string.Empty;
        public ulong FromId { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public ulong ChannelId { get; set; }
    }
}