#nullable enable

namespace Slipstream.Shared.Events.Utility
{
    public class PlayAudio : IEvent
    {
        public string EventType => "PlayAudio";
        public string? Filename { get; set; }
        public float? Volume{ get; set; }
    }
}
