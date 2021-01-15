using NAudio.Wave;
using Serilog;
using Slipstream.Shared;
using Slipstream.Shared.Events.Audio;
using Slipstream.Shared.Factories;
using System;
using System.IO;
using System.Speech.Synthesis;
using System.Threading;
using EventHandler = Slipstream.Shared.EventHandler;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    internal class AudioPlugin : BasePlugin
    {
        private readonly ILogger Logger;
        private readonly string Path;
        private readonly SpeechSynthesizer Synthesizer;
        private readonly IAudioEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private WaveOutEvent OutputDevice = new WaveOutEvent();

        public AudioPlugin(string id, ILogger logger, IEventBus eventBus, IAudioEventFactory eventFactory, IAudioConfiguration audioConfiguration) : base(id, "AudioPlugin", "AudioPlugin", id, true)
        {
            Logger = logger;
            EventBus = eventBus;
            EventFactory = eventFactory;

            Path = audioConfiguration.AudioPath;

            Synthesizer = new SpeechSynthesizer();
            Synthesizer.SetOutputToDefaultAudioDevice();

            var Audio = EventHandler.Get<Shared.EventHandlers.Audio>();

            Audio.OnAudioCommandSay += (_, e) => OnAudioCommandSay(e.Event);
            Audio.OnAudioCommandPlay += (_, e) => OnAudioCommandPlay(e.Event);
            Audio.OnAudioCommandSendDevices += (_, e) => OnAudioCommandSendDevices(e.Event);
            Audio.OnAudioCommandSetOutputDevice += (_, e) => OnAudioCommandSetOutputDevice(e.Event);
        }

        private void OnAudioCommandSetOutputDevice(AudioCommandSetOutputDevice @event)
        {
            if (@event.PluginId != Id)
                return;

            OutputDevice = new WaveOutEvent() { DeviceNumber = @event.DeviceIdx };
        }

        private void OnAudioCommandSendDevices(AudioCommandSendDevices @event)
        {
            if (@event.PluginId != Id)
                return;

            for (int n = -1; n < WaveOut.DeviceCount; n++)
            {
                var caps = WaveOut.GetCapabilities(n);

                EventBus.PublishEvent(EventFactory.CreateAudioOutputDevice(Id, caps.ProductName, n));
            }
        }

        private void OnAudioCommandPlay(Shared.Events.Audio.AudioCommandPlay @event)
        {
            if (@event.PluginId != Id)
                return;

            var filename = @event.Filename;
            var volume = @event.Volume;
            var filePath = System.IO.Path.Combine(Path, filename);

            try
            {
                using var audioFile = new AudioFileReader(filePath);

                Play(new AudioFileReader(filePath), (float)volume);
            }
            catch (Exception ex)
            {
                Logger.Error("Playing audio file failed: {Message}", ex.Message);
            }
        }

        private void OnAudioCommandSay(Shared.Events.Audio.AudioCommandSay @event)
        {
            if (@event.PluginId != Id)
                return;

            using var stream = new MemoryStream();

            Synthesizer.SetOutputToWaveStream(stream);
            Synthesizer.Speak(@event.Message);

            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            Play(new WaveFileReader(stream), (float)@event.Volume);
        }

        private void Play(WaveStream stream, float volume)
        {
            OutputDevice.Init(stream);
            OutputDevice.Volume = volume;
            OutputDevice.Play();
            while (OutputDevice.PlaybackState == PlaybackState.Playing)
            {
                Thread.Sleep(100);
            }
        }
    }
}