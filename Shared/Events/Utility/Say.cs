#nullable enable

namespace Slipstream.Shared.Events.Utility
{
    public class Say : IEvent
    {
        public string EventType => "Say";
        public string? Message { get; set; }
        public float? Volume { get; set; }
    }
}
