#nullable enable

using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.Internal.Events
{
    public class InternalCommandPluginStates : IEvent
    {
        public string EventType => "InternalCommandPluginStates";
        public ulong Uptime { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is InternalCommandPluginStates states &&
                   EventType == states.EventType;
        }

        public override int GetHashCode()
        {
            int hashCode = -441302714;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            return hashCode;
        }
    }
}