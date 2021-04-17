using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.UI.Events
{
    public class UICommandDeleteButton : IEvent
    {
        public string EventType => "UICommandDeleteButton";
        public ulong Uptime { get; set; }
        public string Text { get; set; } = "";

        public override bool Equals(object obj)
        {
            return obj is UICommandDeleteButton button &&
                   EventType == button.EventType &&
                   Text == button.Text;
        }

        public override int GetHashCode()
        {
            int hashCode = 25878420;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Text);
            return hashCode;
        }
    }
}