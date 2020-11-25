#nullable enable

namespace Slipstream.Shared.Events.Twitch
{
    public class TwitchReceivedCommand : IEvent
    {
        public string EventType => "TwitchReceivedCommand";
        public string From { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool Moderator { get; set; }
        public bool Subscriber { get; set; }
        public bool Vip { get; set; }
        public bool Broadcaster { get; set; }
    }
}
