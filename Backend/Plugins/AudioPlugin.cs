using NAudio.Wave;
using Serilog;
using Slipstream.Shared;
using System;
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

        public AudioPlugin(string id, ILogger logger, IAudioConfiguration audioConfiguration) : base(id, "AudioPlugin", "AudioPlugin", id, true)
        {
            Logger = logger;

            Path = audioConfiguration.AudioPath;

            Synthesizer = new SpeechSynthesizer();
            Synthesizer.SetOutputToDefaultAudioDevice();

            var Audio = EventHandler.Get<Shared.EventHandlers.Audio>();

            Audio.OnAudioCommandSay += (_, e) => OnAudioCommandSay(e.Event);
            Audio.OnAudioCommandPlay += (_, e) => OnAudioCommandPlay(e.Event);
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
                using var outputDevice = new WaveOutEvent();
                outputDevice.Init(audioFile);
                outputDevice.Volume = (float)volume;
                outputDevice.Play();
                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(100);
                }
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

            Synthesizer.Volume = Math.Min((int)(100 * @event.Volume), 100);
            Synthesizer.Speak(@event.Message);
        }
    }
}