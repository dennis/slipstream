using Slipstream.Shared;

namespace Slipstream.Components.Audio.Events
{
    public class AudioCommandSetOutputDevice : IEvent
    {
        public string EventType => nameof(AudioCommandSetOutputDevice);
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        
        public int DeviceIdx { get; set; } = -1;
    }
}