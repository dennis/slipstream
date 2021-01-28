using NAudio.Wave;
using Serilog;
using Slipstream.Backend.Plugins;
using Slipstream.Components.Audio.Events;
using Slipstream.Shared;
using Slipstream.Shared.Factories;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using System;
using System.IO;
using System.Speech.Synthesis;
using System.Threading;

#nullable enable

namespace Slipstream.Components.Audio.Plugins
{
    internal class AudioPlugin : BasePlugin
    {
        public static DictionaryValidator ConfigurationValidator { get; }

        private readonly ILogger Logger;
        private readonly string Path;
        private readonly SpeechSynthesizer Synthesizer;
        private readonly IAudioEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private WaveOutEvent OutputDevice = new WaveOutEvent();

        static AudioPlugin()
        {
            ConfigurationValidator = new DictionaryValidator()
                .PermitString("path")
                .PermitLong("output");
        }

        public AudioPlugin(IEventHandlerController eventHandlerController, string id, ILogger logger, IEventBus eventBus, IAudioEventFactory eventFactory, Parameters configuration) : base(eventHandlerController, id, "AudioPlugin", id, true)
        {
            Logger = logger;
            EventBus = eventBus;
            EventFactory = eventFactory;

            ConfigurationValidator.Validate(configuration);

            Path = configuration.ExtractOrDefault("path", "Audio");

            Synthesizer = new SpeechSynthesizer();
            var outputDeviceIdx = configuration.ExtractOrDefault("output", -1);

            if (outputDeviceIdx == -1)
            {
                Synthesizer.SetOutputToDefaultAudioDevice();
            }
            else
            {
                OutputDevice = new WaveOutEvent() { DeviceNumber = outputDeviceIdx };
            }

            var Audio = EventHandlerController.Get<Components.Audio.EventHandler.AudioEventHandler>();

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

        private void OnAudioCommandPlay(AudioCommandPlay @event)
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

        private void OnAudioCommandSay(AudioCommandSay @event)
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