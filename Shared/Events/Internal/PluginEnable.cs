#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class PluginEnable : IEvent
    {
        public string EventType => "PluginEnable";
        public string Id { get; set; } = "INVALID-PLUGIN-ID";
    }
}
