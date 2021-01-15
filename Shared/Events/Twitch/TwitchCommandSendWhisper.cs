#nullable enable

using System.Collections.Generic;

namespace Slipstream.Shared.Events.Twitch
{
    public class TwitchCommandSendWhisper : IEvent
    {
        public string EventType => "TwitchCommandSendWhisper";
        public bool ExcludeFromTxrx => false;
        public ulong Uptime { get; set; }
        public string To { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            return obj is TwitchCommandSendWhisper whisper &&
                   EventType == whisper.EventType &&
                   ExcludeFromTxrx == whisper.ExcludeFromTxrx &&
                   To == whisper.To &&
                   Message == whisper.Message;
        }

        public override int GetHashCode()
        {
            int hashCode = 855176974;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(To);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Message);
            return hashCode;
        }
    }
}
