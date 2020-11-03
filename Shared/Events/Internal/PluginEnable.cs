#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class PluginEnable : IEvent
    {
        public string? PluginName { get; set; }
    }
}
