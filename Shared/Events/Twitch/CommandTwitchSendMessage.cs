#nullable enable

namespace Slipstream.Shared.Events.Twitch
{
    public class CommandTwitchSendMessage : IEvent
    {
        public string EventType => "CommandTwitchSendMessage";
        public string Message { get; set; } = string.Empty;
    }
}
