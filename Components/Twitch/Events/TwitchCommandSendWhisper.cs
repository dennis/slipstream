#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Twitch.Events
{
    public class TwitchCommandSendWhisper : IEvent
    {
        public string EventType => nameof(TwitchCommandSendWhisper);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public string To { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}