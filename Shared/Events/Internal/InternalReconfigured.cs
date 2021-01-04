#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class InternalReconfigured : IEvent
    {
        public string EventType => "InternalReconfigured";
        public bool ExcludeFromTxrx => true;
    }
}
