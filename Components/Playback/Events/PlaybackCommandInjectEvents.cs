using Slipstream.Shared;

namespace Slipstream.Components.Playback.Events
{
    public class PlaybackCommandInjectEvents : IEvent
    {
        public string EventType => nameof(PlaybackCommandInjectEvents);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public string Filename { get; set; } = string.Empty;
    }
}