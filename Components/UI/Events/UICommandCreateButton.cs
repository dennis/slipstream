using Slipstream.Shared;

namespace Slipstream.Components.UI.Events
{
    public class UICommandCreateButton : IEvent
    {
        public string EventType => nameof(UICommandCreateButton);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public string Text { get; set; } = "";
    }
}