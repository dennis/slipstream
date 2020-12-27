#nullable enable

namespace Slipstream.Shared.Events.Utility
{
    public class CommandPlayAudio : IEvent
    {
        public string EventType => "CommandPlayAudio";
        public string? Filename { get; set; }
        public float? Volume { get; set; }
    }
}
