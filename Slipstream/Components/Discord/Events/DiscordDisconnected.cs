using Slipstream.Shared;

namespace Slipstream.Components.Discord.Events
{
    public class DiscordDisconnected : IEvent
    {
        public string EventType => nameof(DiscordDisconnected);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
    }
}