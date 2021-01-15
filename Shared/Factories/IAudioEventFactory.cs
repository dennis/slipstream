using Slipstream.Shared.Events.Audio;

#nullable enable

namespace Slipstream.Shared.Factories
{
    public interface IAudioEventFactory
    {
        AudioCommandPlay CreateAudioCommandPlay(string pluginId, string filename, float volume);

        AudioCommandSay CreateAudioCommandSay(string pluginId, string message, float volume);

        AudioCommandSendDevices CreateAudioCommandSendDevices(string pluginId);

        AudioOutputDevice CreateAudioOutputDevice(string pluginId, string product, int deviceIdx);

        AudioCommandSetOutputDevice CreateAudioCommandSetOutputDevice(string pluginId, int deviceIdx);
    }
}