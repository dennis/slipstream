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
    }
}
