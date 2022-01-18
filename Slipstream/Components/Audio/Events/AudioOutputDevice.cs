#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Audio.Events
{
    public class AudioOutputDevice : IEvent
    {
        public string EventType => nameof(AudioOutputDevice);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public string Product { get; set; } = string.Empty;
        public int DeviceIdx { get; set; } = -1;
    }
}