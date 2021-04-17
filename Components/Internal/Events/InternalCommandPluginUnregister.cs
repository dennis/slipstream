#nullable enable

using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.Internal.Events
{
    public class InternalCommandPluginUnregister : IEvent
    {
        public string EventType => "InternalCommandPluginUnregister";
        public ulong Uptime { get; set; }
        public string Id { get; set; } = "INVALID-PLUGIN-ID";

        public override bool Equals(object? obj)
        {
            return obj is InternalCommandPluginUnregister unregister &&
                   EventType == unregister.EventType &&
                   Id == unregister.Id;
        }

        public override int GetHashCode()
        {
            int hashCode = -94409930;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            return hashCode;
        }
    }
}