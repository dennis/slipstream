#nullable enable

using Slipstream.Components.Audio.Events;
using Slipstream.Shared;

namespace Slipstream.Components.Audio.EventFactory
{
    public class AudioEventFactory : IAudioEventFactory
    {
        public AudioCommandPlay CreateAudioCommandPlay(IEventEnvelope envelope, string filename, float volume)
        {
            return new AudioCommandPlay { Envelope = envelope, Filename = filename, Volume = volume };
        }

        public AudioCommandSay CreateAudioCommandSay(IEventEnvelope envelope, string message, float volume)
        {
            return new AudioCommandSay { Envelope = envelope, Message = message, Volume = volume };
        }

        public AudioCommandSendDevices CreateAudioCommandSendDevices(IEventEnvelope envelope)
        {
            return new AudioCommandSendDevices { Envelope = envelope };
        }

        public AudioOutputDevice CreateAudioOutputDevice(IEventEnvelope envelope, string product, int deviceIdx)
        {
            return new AudioOutputDevice { DeviceIdx = deviceIdx, Envelope = envelope, Product = product };
        }

        public AudioCommandSetOutputDevice CreateAudioCommandSetOutputDevice(IEventEnvelope envelope, int deviceIdx)
        {
            return new AudioCommandSetOutputDevice { Envelope = envelope, DeviceIdx = deviceIdx };
        }
    }
}