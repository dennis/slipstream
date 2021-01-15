#nullable enable


using System.Collections.Generic;

namespace Slipstream.Shared.Events.Audio
{
    public class AudioOutputDevice : IEvent
    {
        public string EventType => "AudioOutputDevice";
        public bool ExcludeFromTxrx => false;
        public string PluginId { get; set; } = "INVAILD-PLUGIN-ID";
        public string Product { get; set; } = string.Empty;
        public int DeviceIdx { get; set; } = -1;
        public ulong Uptime { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is AudioOutputDevice device &&
                   EventType == device.EventType &&
                   ExcludeFromTxrx == device.ExcludeFromTxrx &&
                   PluginId == device.PluginId &&
                   Product == device.Product &&
                   DeviceIdx == device.DeviceIdx;
        }

        public override int GetHashCode()
        {
            int hashCode = -1602761893;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + ExcludeFromTxrx.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PluginId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Product);
            hashCode = hashCode * -1521134295 + DeviceIdx.GetHashCode();
            return hashCode;
        }
    }
}
