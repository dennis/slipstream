using Slipstream.Shared.Events.Audio;

#nullable enable

namespace Slipstream.Shared.Factories
{
    public class AudioEventFactory : IAudioEventFactory
    {
        public AudioCommandPlay CreateAudioCommandPlay(string pluginId, string filename, float volume)
        {
            return new AudioCommandPlay { PluginId = pluginId, Filename = filename, Volume = volume };
        }

        public AudioCommandSay CreateAudioCommandSay(string pluginId, string message, float volume)
        {
            return new AudioCommandSay { PluginId = pluginId, Message = message, Volume = volume };
        }

        public AudioCommandSendDevices CreateAudioCommandSendDevices(string pluginId)
        {
            return new AudioCommandSendDevices{ PluginId = pluginId };
        }

        public AudioOutputDevice CreateAudioOutputDevice(string pluginId, string product, int deviceIdx)
        {
            return new AudioOutputDevice { DeviceIdx = deviceIdx, PluginId = pluginId, Product = product };
        }

        public AudioCommandSetOutputDevice CreateAudioCommandSetOutputDevice(string pluginId, int deviceIdx)
        {
            return new AudioCommandSetOutputDevice { PluginId = pluginId, DeviceIdx = deviceIdx };
        }
    }
}