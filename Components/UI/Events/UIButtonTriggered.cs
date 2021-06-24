using Slipstream.Shared;

namespace Slipstream.Components.UI.Events
{
    public class UIButtonTriggered : IEvent
    {
        public string EventType => nameof(UIButtonTriggered);
        
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public string Text { get; set; } = "INVALID-NAME";
    }
}