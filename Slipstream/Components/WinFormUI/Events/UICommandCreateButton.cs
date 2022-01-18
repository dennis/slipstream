using Slipstream.Shared;

namespace Slipstream.Components.WinFormUI.Events
{
    public class WinFormUICommandCreateButton : IEvent
    {
        public string EventType => nameof(WinFormUICommandCreateButton);

        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();

        public string Text { get; set; } = "";
    }
}