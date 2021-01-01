#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class InternalCommandPluginUnregister : IEvent
    {
        public string EventType => "InternalCommandPluginUnregister";
        public bool ExcludeFromTxrx => true;
        public string Id { get; set; } = "INVALID-PLUGIN-ID";
    }
}
