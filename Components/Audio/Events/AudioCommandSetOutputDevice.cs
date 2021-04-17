using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Components.Audio.Events
{
    public class AudioCommandSetOutputDevice : IEvent
    {
        public string EventType => "AudioCommandSetOutputDevice";
        public string InstanceId { get; set; } = "INVALID-INSTANCE-ID";
        public int DeviceIdx { get; set; } = -1;
        public ulong Uptime { get; set; }

        public override bool Equals(object obj)
        {
            return obj is AudioCommandSetOutputDevice device &&
                   EventType == device.EventType &&
                   InstanceId == device.InstanceId &&
                   DeviceIdx == device.DeviceIdx;
        }

        public override int GetHashCode()
        {
            int hashCode = 1390535527;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(InstanceId);
            hashCode = hashCode * -1521134295 + DeviceIdx.GetHashCode();
            return hashCode;
        }
    }
}