#nullable enable

using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.Twitch.Events
{
    public class TwitchReceivedWhisper : IEvent
    {
        public string EventType => "TwitchReceivedWhisper";
        public ulong Uptime { get; set; }
        public string InstanceId { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            return obj is TwitchReceivedWhisper whisper &&
                   EventType == whisper.EventType &&
                   InstanceId == whisper.InstanceId &&
                   From == whisper.From &&
                   Message == whisper.Message;
        }

        public override int GetHashCode()
        {
            int hashCode = -370540573;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(InstanceId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(From);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Message);
            return hashCode;
        }
    }
}