#nullable enable

using System.Collections.Generic;

namespace Slipstream.Shared.Events.Internal
{
    public class InternalCommandPluginUnregister : IEvent
    {
        public string EventType => "InternalCommandPluginUnregister";
        public bool ExcludeFromTxrx => true;
        public string Id { get; set; } = "INVALID-PLUGIN-ID";

        public override bool Equals(object? obj)
        {
            return obj is InternalCommandPluginUnregister unregister &&
                   EventType == unregister.EventType &&
                   ExcludeFromTxrx == unregister.ExcludeFromTxrx &&
                   Id == unregister.Id;
        }

        public override int GetHashCode()
        {
            int hashCode = -94409930;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            return hashCode;
        }
    }
}
