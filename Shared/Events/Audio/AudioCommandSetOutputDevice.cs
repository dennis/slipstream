using System.Collections.Generic;

namespace Slipstream.Shared.Events.Audio
{
    public class AudioCommandSetOutputDevice : IEvent
    {
        public string EventType => "AudioCommandSetOutputDevice";
        public bool ExcludeFromTxrx => false;
        public string PluginId { get; set; } = "INVALID-PLUGIN-ID";
        public int DeviceIdx { get; set; } = -1;
        public ulong Uptime { get; set; }

        public override bool Equals(object obj)
        {
            return obj is AudioCommandSetOutputDevice device &&
                   EventType == device.EventType &&
                   ExcludeFromTxrx == device.ExcludeFromTxrx &&
                   PluginId == device.PluginId &&
                   DeviceIdx == device.DeviceIdx;
        }

        public override int GetHashCode()
        {
            int hashCode = 1390535527;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PluginId);
            hashCode = hashCode * -1521134295 + DeviceIdx.GetHashCode();
            return hashCode;
        }
    }
}