#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class CommandPluginEnable : IEvent
    {
        public string EventType => "CommandPluginEnable";
        public string Id { get; set; } = "INVALID-PLUGIN-ID";
    }
}
