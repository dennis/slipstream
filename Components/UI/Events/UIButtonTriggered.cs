using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.UI.Events
{
    public class UIButtonTriggered : IEvent
    {
        public string EventType => "UIButtonTriggered";
        public bool ExcludeFromTxrx => false;
        public ulong Uptime { get; set; }
        public string Text { get; set; } = "INVALID-NAME";

        public override bool Equals(object obj)
        {
            return obj is UIButtonTriggered triggered &&
                   EventType == triggered.EventType &&
                   ExcludeFromTxrx == triggered.ExcludeFromTxrx &&
                   Text == triggered.Text;
        }

        public override int GetHashCode()
        {
            int hashCode = 25878420;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Text);
            return hashCode;
        }
    }
}