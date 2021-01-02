using NAudio.Wave;
using Slipstream.Shared;
using Slipstream.Shared.Events.Setting;
using Slipstream.Shared.Events.UI;
using System;
using System.Speech.Synthesis;
using System.Threading;
using EventHandler = Slipstream.Shared.EventHandler;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    class AudioPlugin : BasePlugin
    {
        private readonly IEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private string? Path;
        private readonly SpeechSynthesizer Synthesizer;

        public AudioPlugin(string id, IEventFactory eventFactory, IEventBus eventBus, AudioSettings settings) : base(id, "AudioPlugin", "AudioPlugin", "Audio")
        {
            EventFactory = eventFactory;
            EventBus = eventBus;

            EventHandler_OnSettingAudioSettings(settings);

            Synthesizer = new SpeechSynthesizer();
            Synthesizer.SetOutputToDefaultAudioDevice();

            EventHandler.OnAudioCommandSay += EventHandler_OnUtilitySay;
            EventHandler.OnAudioCommandPlay += EventHandler_OnAudioCommandPlay;
            EventHandler.OnSettingAudioSettings += (s, e) => EventHandler_OnSettingAudioSettings(e.Event);
        }

        private void EventHandler_OnSettingAudioSettings(AudioSettings e)
        {
            Path = e.Path;
        }

        private void EventHandler_OnAudioCommandPlay(Shared.EventHandler source, Shared.EventHandler.EventHandlerArgs<Shared.Events.Audio.AudioCommandPlay> e)
        {
            var filename = e.Event.Filename;
            var volume = e.Event.Volume;

            if (filename == null || volume == null || Path == null)
                return;

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
                EventBus.PublishEvent(EventFactory.CreateUICommandWriteToConsole("Playing audio file failed: " + ex.Message));
            }
        }

        private void EventHandler_OnUtilitySay(Shared.EventHandler source, Shared.EventHandler.EventHandlerArgs<Shared.Events.Audio.AudioCommandSay> e)
        {
            if (e.Event == null || e.Event.Message == null || e.Event.Volume == null)
                return;

            Synthesizer.Volume = Math.Min((int)(100 * e.Event.Volume), 100);
            Synthesizer.Speak(e.Event.Message);
        }
    }
}
