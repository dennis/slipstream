using Slipstream.Shared;

namespace Slipstream.Components.Twitch.Events
{
    public class TwitchConnected : IEvent
    {
        public string EventType => nameof(TwitchConnected);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public string InstanceId { get; set; } = string.Empty;
    }
}