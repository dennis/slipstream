#nullable enable

using System.Collections.Generic;

namespace Slipstream.Shared.Events.Internal
{
    public class InternalPluginState : IEvent
    {
        public string EventType => "InternalPluginState";
        public bool ExcludeFromTxrx => true;
        public string Id { get; set; } = "INVALID-PLUGIN-ID";
        public string PluginName { get; set; } = "INVALID-PLUGIN-NAME";
        public string DisplayName { get; set; } = "INVALID-DISPLAY-NAME";
        public string PluginStatus { get; set; } = "INVALID-STATE";

        public override bool Equals(object? obj)
        {
            return obj is InternalPluginState state &&
                   EventType == state.EventType &&
                   ExcludeFromTxrx == state.ExcludeFromTxrx &&
                   Id == state.Id &&
                   PluginName == state.PluginName &&
                   DisplayName == state.DisplayName &&
                   PluginStatus == state.PluginStatus;
        }

        public override int GetHashCode()
        {
            int hashCode = 1261067873;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PluginName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DisplayName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PluginStatus);
            return hashCode;
        }
    }
}
