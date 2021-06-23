using Slipstream.Shared;

namespace Slipstream.Components.Twitch.Events
{
    public class TwitchDisconnected : IEvent
    {
        public string EventType => nameof(TwitchDisconnected);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
    }
}