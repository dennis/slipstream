using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.UI.Events
{
    public class UICommandDeleteButton : IEvent
    {
        public string EventType => nameof(UICommandDeleteButton);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        
        public string Text { get; set; } = "";
    }
}