using Slipstream.Shared.Events.Audio;

#nullable enable

namespace Slipstream.Shared.Factories
{
    public class AudioEventFactory : IAudioEventFactory
    {
        public AudioCommandPlay CreateAudioCommandPlay(string filename, float? volume)
        {
            return new AudioCommandPlay { Filename = filename, Volume = volume };
        }

        public AudioCommandSay CreateAudioCommandSay(string message, float? volume)
        {
            return new AudioCommandSay { Message = message, Volume = volume };
        }
    }
}
