#nullable enable

using Slipstream.Components.Audio.Events;
using Slipstream.Shared;

namespace Slipstream.Components.Audio
{
    public interface IAudioEventFactory
    {
        AudioCommandPlay CreateAudioCommandPlay(IEventEnvelope envelope, string filename, float volume);

        AudioCommandSay CreateAudioCommandSay(IEventEnvelope envelope, string message, float volume);

        AudioCommandSendDevices CreateAudioCommandSendDevices(IEventEnvelope envelope);

        AudioOutputDevice CreateAudioOutputDevice(IEventEnvelope envelope, string product, int deviceIdx);

        AudioCommandSetOutputDevice CreateAudioCommandSetOutputDevice(IEventEnvelope envelope, int deviceIdx);
    }
}