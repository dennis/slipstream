#nullable enable

using System.Collections.Generic;

namespace Slipstream.Shared.Events.Internal
{
    public class InternalInitialized : IEvent
    {
        public string EventType => "InternalInitialized";
        public bool ExcludeFromTxrx => true;

        public override bool Equals(object? obj)
        {
            return obj is InternalInitialized initialized &&
                   EventType == initialized.EventType &&
                   ExcludeFromTxrx == initialized.ExcludeFromTxrx;
        }

        public override int GetHashCode()
        {
            int hashCode = -441302714;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            return hashCode;
        }
    }
}
