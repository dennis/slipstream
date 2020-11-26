#nullable enable

namespace Slipstream.Shared.Events.Twitch
{
    public class TwitchSendMessage : IEvent
    {
        public string EventType => "TwitchSendMessage";
        public string Message { get; set; } = string.Empty;
    }
}
