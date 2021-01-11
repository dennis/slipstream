using Slipstream.Shared.Events.Audio;

#nullable enable

namespace Slipstream.Shared.Factories
{
    public interface IAudioEventFactory
    {
        AudioCommandPlay CreateAudioCommandPlay(string filename, float? volume);
        AudioCommandSay CreateAudioCommandSay(string message, float? volume);
    }
}
