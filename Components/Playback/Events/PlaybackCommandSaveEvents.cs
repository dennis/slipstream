using Slipstream.Shared;

namespace Slipstream.Components.Playback.Events
{
    public class PlaybackCommandSaveEvents : IEvent
    {
        public string EventType => nameof(PlaybackCommandSaveEvents);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public string Filename { get; set; } = string.Empty;
    }
}