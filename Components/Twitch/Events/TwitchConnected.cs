using Slipstream.Shared;

namespace Slipstream.Components.Twitch.Events
{
    public class TwitchConnected : IEvent
    {
        public string EventType => nameof(TwitchConnected);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
    }
}