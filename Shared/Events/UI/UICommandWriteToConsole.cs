#nullable enable

using System.Collections.Generic;

namespace Slipstream.Shared.Events.UI
{
    public class UICommandWriteToConsole : IEvent
    {
        public string EventType => "UICommandWriteToConsole";
        public bool ExcludeFromTxrx => true;
        public string? Message { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is UICommandWriteToConsole console &&
                   EventType == console.EventType &&
                   ExcludeFromTxrx == console.ExcludeFromTxrx &&
                   Message == console.Message;
        }

        public override int GetHashCode()
        {
            int hashCode = 1904577466;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string?>.Default.GetHashCode(Message);
            return hashCode;
        }
    }
}
