#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class PluginDisable : IEvent
    {
        public string? PluginName { get; set; }
    }
}
