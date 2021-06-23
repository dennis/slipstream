#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Audio.Events
{
    public class AudioCommandSay : IEvent
    {
        public string EventType => nameof(AudioCommandSay);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public string Message { get; set; } = string.Empty;
        public float Volume { get; set; } = 1.0f;
    }
}