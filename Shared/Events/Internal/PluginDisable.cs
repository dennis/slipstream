#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class PluginDisable : IEvent
    {
        public string EventType => "PluginDisable";
        public string Id { get; set; } = "INVALID-PLUGIN-ID";
    }
}
