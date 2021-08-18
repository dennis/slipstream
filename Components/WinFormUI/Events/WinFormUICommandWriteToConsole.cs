#nullable enable

using Slipstream.Shared;

namespace Slipstream.Components.WinFormUI.Events
{
    public class WinFormUICommandWriteToConsole : IEvent
    {
        public string EventType => nameof(WinFormUICommandWriteToConsole);

        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        public string Message { get; set; } = string.Empty;
        public bool Error { get; set; } = false;
    }
}