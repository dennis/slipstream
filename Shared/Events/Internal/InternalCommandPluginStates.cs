#nullable enable

using System.Collections.Generic;

namespace Slipstream.Shared.Events.Internal
{
    public class InternalCommandPluginStates : IEvent
    {
        public string EventType => "InternalCommandPluginStates";
        public bool ExcludeFromTxrx => true;

        public override bool Equals(object? obj)
        {
            return obj is InternalCommandPluginStates states &&
                   EventType == states.EventType &&
                   ExcludeFromTxrx == states.ExcludeFromTxrx;
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
