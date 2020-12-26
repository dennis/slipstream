#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class PluginStatesRequest : IEvent
    {
        public string EventType => "PluginStatesRequest";
    }
}
