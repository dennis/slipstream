#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class InternalCommandPluginDisable : IEvent
    {
        public string EventType => "InternalCommandPluginDisable";
        public bool ExcludeFromTxrx => true;
        public string Id { get; set; } = "INVALID-PLUGIN-ID";
    }
}
