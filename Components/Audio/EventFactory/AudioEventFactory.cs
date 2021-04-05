#nullable enable

using Slipstream.Components.Audio.Events;

namespace Slipstream.Components.Audio.EventFactory
{
    public class AudioEventFactory : IAudioEventFactory
    {
        public AudioCommandPlay CreateAudioCommandPlay(string instanceId, string filename, float volume)
        {
            return new AudioCommandPlay { InstanceId = instanceId, Filename = filename, Volume = volume };
        }

        public AudioCommandSay CreateAudioCommandSay(string instanceId, string message, float volume)
        {
            return new AudioCommandSay { InstanceId = instanceId, Message = message, Volume = volume };
        }

        public AudioCommandSendDevices CreateAudioCommandSendDevices(string instanceId)
        {
            return new AudioCommandSendDevices { InstanceId = instanceId };
        }

        public AudioOutputDevice CreateAudioOutputDevice(string instanceId, string product, int deviceIdx)
        {
            return new AudioOutputDevice { DeviceIdx = deviceIdx, InstanceId = instanceId, Product = product };
        }

        public AudioCommandSetOutputDevice CreateAudioCommandSetOutputDevice(string instanceId, int deviceIdx)
        {
            return new AudioCommandSetOutputDevice { InstanceId = instanceId, DeviceIdx = deviceIdx };
        }
    }
}