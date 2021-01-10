#nullable enable

using System.Collections.Generic;

namespace Slipstream.Shared.Events.Internal
{
    public class InternalCommandReconfigure : IEvent
    {
        public string EventType => "InternalCommandReconfigure";
        public bool ExcludeFromTxrx => true;

        public override bool Equals(object? obj)
        {
            return obj is InternalCommandReconfigure reconfigured &&
                   EventType == reconfigured.EventType &&
                   ExcludeFromTxrx == reconfigured.ExcludeFromTxrx;
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
