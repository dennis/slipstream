#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class InternalInitialized : IEvent
    {
        public string EventType => "InternalInitialized";
        public bool ExcludeFromTxrx => true;
    }
}
