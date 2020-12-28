#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class CommandPluginDisable : IEvent
    {
        public string EventType => "CommandPluginDisable";
        public string Id { get; set; } = "INVALID-PLUGIN-ID";
    }
}
