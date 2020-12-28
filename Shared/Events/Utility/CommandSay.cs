#nullable enable

namespace Slipstream.Shared.Events.Utility
{
    public class CommandSay : IEvent
    {
        public string EventType => "CommandSay";
        public string? Message { get; set; }
        public float? Volume { get; set; }
    }
}
