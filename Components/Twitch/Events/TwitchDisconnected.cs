using Slipstream.Shared;

namespace Slipstream.Components.Twitch.Events
{
    public class TwitchDisconnected : IEvent
    {
        public string EventType => nameof(TwitchDisconnected);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
    }
}