#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Twitch.Events
{
    public class TwitchCommandSendMessage : IEvent
    {
        public string EventType => nameof(TwitchCommandSendMessage);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public string Message { get; set; } = string.Empty;
    }
}