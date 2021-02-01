#nullable enable

using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.Internal.Events
{
    public class InternalCommandPluginRegister : IEvent
    {
        public string EventType => "InternalCommandPluginRegister";
        public bool ExcludeFromTxrx => true;
        public ulong Uptime { get; set; }
        public string Id { get; set; } = "INVALID-PLUGIN-ID";
        public string PluginName { get; set; } = "INVALID-PLUGIN-NAME";
        public string Configuration { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            return obj is InternalCommandPluginRegister register &&
                   EventType == register.EventType &&
                   ExcludeFromTxrx == register.ExcludeFromTxrx &&
                   Id == register.Id &&
                   PluginName == register.PluginName &&
                   Configuration == register.Configuration;
        }

        public override int GetHashCode()
        {
            int hashCode = 302108760;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PluginName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Configuration);
            return hashCode;
        }
    }
}