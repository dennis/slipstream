#nullable enable

namespace Slipstream.Shared.Events.Utility
{
    public class CommandWriteToConsole : IEvent
    {
        public string EventType => "CommandWriteToConsole";
        public string? Message { get; set; }
    }
}
