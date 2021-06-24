using Slipstream.Shared;

namespace Slipstream.Components.Discord.Events
{
    public class DiscordConnected : IEvent
    {
        public string EventType => nameof(DiscordConnected);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
    }
}