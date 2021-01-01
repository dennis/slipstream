#nullable enable

namespace Slipstream.Shared.Events.Audio
{
    public class AudioCommandPlay : IEvent
    {
        public string EventType => "AudioCommandPlay";
        public bool ExcludeFromTxrx => true;
        public string? Filename { get; set; }
        public float? Volume { get; set; }
    }
}
