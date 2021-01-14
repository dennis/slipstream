#nullable enable

using System.Collections.Generic;

namespace Slipstream.Shared.Events.Twitch
{
    public class TwitchCommandSendMessage : IEvent
    {
        public string EventType => "TwitchCommandSendMessage";
        public bool ExcludeFromTxrx => false;
        public ulong Uptime { get; set; }
        public string Message { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            return obj is TwitchCommandSendMessage message &&
                   EventType == message.EventType &&
                   ExcludeFromTxrx == message.ExcludeFromTxrx &&
                   Message == message.Message;
        }

        public override int GetHashCode()
        {
            int hashCode = 1904577466;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Message);
            return hashCode;
        }
    }
}
