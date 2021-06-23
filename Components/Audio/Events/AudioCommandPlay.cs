#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Audio.Events
{
    public class AudioCommandPlay : IEvent
    {
        public string EventType => nameof(AudioCommandPlay);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public string Filename { get; set; } = string.Empty;
        public float Volume { get; set; } = 1.0f;
    }
}