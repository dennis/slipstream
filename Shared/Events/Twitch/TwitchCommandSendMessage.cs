#nullable enable

namespace Slipstream.Shared.Events.Twitch
{
    public class TwitchCommandSendMessage : IEvent
    {
        public string EventType => "TwitchCommandSendMessage";
        public bool ExcludeFromTxrx => false;
        public string Message { get; set; } = string.Empty;
    }
}
