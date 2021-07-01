using Slipstream.Shared;

namespace Slipstream.Components.WinFormUI.Events
{
    public class WinFormUICommandDeleteButton : IEvent
    {
        public string EventType => nameof(WinFormUICommandDeleteButton);

        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();

        public string Text { get; set; } = "";
    }
}