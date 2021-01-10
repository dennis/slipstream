#nullable enable

using System.Collections.Generic;

namespace Slipstream.Shared.Events.Twitch
{
    public class TwitchReceivedWhisper : IEvent
    {
        public string EventType => "TwitchReceivedWhisper";
        public bool ExcludeFromTxrx => false;
        public string From { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            return obj is TwitchReceivedWhisper whisper &&
                   EventType == whisper.EventType &&
                   ExcludeFromTxrx == whisper.ExcludeFromTxrx &&
                   From == whisper.From &&
                   Message == whisper.Message;
        }

        public override int GetHashCode()
        {
            int hashCode = -370540573;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(From);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Message);
            return hashCode;
        }
    }
}
