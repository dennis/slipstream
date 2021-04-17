#nullable enable

using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.Twitch.Events
{
    public class TwitchCommandSendMessage : IEvent
    {
        public string EventType => "TwitchCommandSendMessage";
        public ulong Uptime { get; set; }
        public string Message { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            return obj is TwitchCommandSendMessage message &&
                   EventType == message.EventType &&
                   Message == message.Message;
        }

        public override int GetHashCode()
        {
            int hashCode = 1904577466;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Message);
            return hashCode;
        }
    }
}