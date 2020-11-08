#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class PluginUnregister : IEvent
    {
        public string EventType => "PluginUnregister";
        public string Id { get; set; } = "INVALID-PLUGIN-ID";
    }
}
