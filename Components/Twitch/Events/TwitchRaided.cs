using Slipstream.Shared;

namespace Slipstream.Components.Twitch.Events
{
    public class TwitchRaided : IEvent
    {
        public string EventType => nameof(TwitchRaided);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public string Name { get; set; } = string.Empty;
        public int ViewerCount { get; set; }
    }
}