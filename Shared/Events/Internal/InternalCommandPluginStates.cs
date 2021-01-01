#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class InternalCommandPluginStates : IEvent
    {
        public string EventType => "InternalCommandPluginStates";
        public bool ExcludeFromTxrx => true;
    }
}
