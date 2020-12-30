#nullable enable

namespace Slipstream.Shared.Events.Utility
{
    public class CommandSay : IEvent
    {
        public string EventType => "CommandSay";
        public bool ExcludeFromTxrx => true;
        public string? Message { get; set; }
        public float? Volume { get; set; }
    }
}
