#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class InternalCommandPluginEnable : IEvent
    {
        public string EventType => "InternalCommandPluginEnable";
        public bool ExcludeFromTxrx => true;
        public string Id { get; set; } = "INVALID-PLUGIN-ID";
    }
}
