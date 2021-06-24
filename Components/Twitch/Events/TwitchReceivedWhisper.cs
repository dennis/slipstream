#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Twitch.Events
{
    public class TwitchReceivedWhisper : IEvent
    {
        public string EventType => nameof(TwitchReceivedWhisper);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public string From { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}