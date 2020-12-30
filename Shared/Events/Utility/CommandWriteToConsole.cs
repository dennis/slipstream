#nullable enable

namespace Slipstream.Shared.Events.Utility
{
    public class CommandWriteToConsole : IEvent
    {
        public string EventType => "CommandWriteToConsole";
        public bool ExcludeFromTxrx => true;
        public string? Message { get; set; }
    }
}
