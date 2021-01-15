#nullable enable

using System.Collections.Generic;

namespace Slipstream.Shared.Events.Internal
{
    public class InternalCommandPluginRegister : IEvent
    {
        public string EventType => "InternalCommandPluginRegister";
        public bool ExcludeFromTxrx => true;
        public ulong Uptime { get; set; }
        public string Id { get; set; } = "INVALID-PLUGIN-ID";
        public string PluginName { get; set; } = "INVALID-PLUGIN-NAME";

        public override bool Equals(object? obj)
        {
            return obj is InternalCommandPluginRegister register &&
                   EventType == register.EventType &&
                   ExcludeFromTxrx == register.ExcludeFromTxrx &&
                   Id == register.Id &&
                   PluginName == register.PluginName;
        }

        public override int GetHashCode()
        {
            int hashCode = -172428057;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PluginName);
            return hashCode;
        }
    }
}
