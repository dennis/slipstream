#nullable enable

namespace Slipstream.Shared.Events.Utility
{
    public class WriteToConsole : IEvent
    {
        public string EventType => "WriteToConsole";
        public string? Message { get; set; }
    }
}
