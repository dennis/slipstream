#nullable enable

namespace Slipstream.Shared.Events.Audio
{
    public class AudioCommandSay : IEvent
    {
        public string EventType => "AudioCommandSay";
        public bool ExcludeFromTxrx => true;
        public string? Message { get; set; }
        public float? Volume { get; set; }
    }
}
