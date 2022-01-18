using Slipstream.Shared;

namespace Slipstream.Components.WinFormUI.Events
{
    public class WinFormUIButtonTriggered : IEvent
    {
        public string EventType => nameof(WinFormUIButtonTriggered);

        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();

        public string Text { get; set; } = "INVALID-NAME";
    }
}