#nullable enable

using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.UI.Events
{
    public class UICommandWriteToConsole : IEvent
    {
        public string EventType => "UICommandWriteToConsole";
        public ulong Uptime { get; set; }
        public string? Message { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is UICommandWriteToConsole console &&
                   EventType == console.EventType &&
                   Message == console.Message;
        }

        public override int GetHashCode()
        {
            int hashCode = 1904577466;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string?>.Default.GetHashCode(Message);
            return hashCode;
        }
    }
}