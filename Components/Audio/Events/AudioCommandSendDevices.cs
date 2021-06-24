#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.Audio.Events
{
    public class AudioCommandSendDevices : IEvent
    {
        public string EventType => nameof(AudioCommandSendDevices);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
    }
}