using System.Collections.Generic;

namespace Slipstream.Shared.Events.Audio
{
    public class AudioCommandSendDevices : IEvent
    {
        public string EventType => "AudioCommandSendDevices";
        public bool ExcludeFromTxrx => false;
        public string PluginId { get; set; } = "INVALID-PLUGIN-ID";
        public ulong Uptime { get ; set ; }

        public override bool Equals(object obj)
        {
            return obj is AudioCommandSendDevices devices &&
                   EventType == devices.EventType &&
                   ExcludeFromTxrx == devices.ExcludeFromTxrx &&
                   PluginId == devices.PluginId;
        }

        public override int GetHashCode()
        {
            int hashCode = -3520915;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PluginId);
            return hashCode;
        }
    }
}