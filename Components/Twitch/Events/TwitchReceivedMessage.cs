#nullable enable

using Slipstream.Shared;
using System.ComponentModel;

namespace Slipstream.Components.Twitch.Events
{
    public class TwitchReceivedMessage : IEvent
    {
        public string EventType => nameof(TwitchReceivedMessage);
        public ulong Uptime { get; set; }
        public IEventEnvelope Envelope { get; set; } = new EventEnvelope();
        

        [Description("User that sent the message")]
        public string From { get; set; } = string.Empty;

        [Description("Message body")]
        public string Message { get; set; } = string.Empty;

        [Description("True if the user is a moderator")]
        public bool Moderator { get; set; }

        [Description("True if the user is a subscriber")]
        public bool Subscriber { get; set; }

        [Description("True if the user is a VIP")]
        public bool Vip { get; set; }

        [Description("True if the user is the broadcaster")]
        public bool Broadcaster { get; set; }
    }
}