#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class CommandPluginUnregister : IEvent
    {
        public string EventType => "CommandPluginUnregister";
        public string Id { get; set; } = "INVALID-PLUGIN-ID";
    }
}
