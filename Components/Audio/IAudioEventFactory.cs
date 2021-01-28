#nullable enable

using Slipstream.Components.Audio.Events;

namespace Slipstream.Components.Audio
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