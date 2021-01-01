#nullable enable

namespace Slipstream.Shared.Events.UI
{
    public class UICommandWriteToConsole : IEvent
    {
        public string EventType => "UICommandWriteToConsole";
        public bool ExcludeFromTxrx => true;
        public string? Message { get; set; }
    }
}
