using Slipstream.Shared;

namespace Slipstream.Components.Discord.Events
{
    public class DiscordDisconnected : IEvent
    {
        public string EventType => nameof(DiscordDisconnected);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
    }
}