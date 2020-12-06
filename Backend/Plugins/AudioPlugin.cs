using NAudio.Wave;
using Slipstream.Shared;
using Slipstream.Shared.Events.Setting;
using Slipstream.Shared.Events.Utility;
using System;
using System.Speech.Synthesis;
using System.Threading;
using EventHandler = Slipstream.Shared.EventHandler;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    class AudioPlugin : BasePlugin
    {
        private readonly IEventBus EventBus;
        private string? Path;
        private readonly SpeechSynthesizer Synthesizer;

        public AudioPlugin(string id, IEventBus eventBus) : base(id, "AudioPlugin", "AudioPlugin", "Core")
        {
            EventBus = eventBus;

            Synthesizer = new SpeechSynthesizer();
            Synthesizer.SetOutputToDefaultAudioDevice();

            EventHandler.OnUtilitySay += EventHandler_OnUtilitySay;
            EventHandler.OnUtilityPlayAudio += EventHandler_OnUtilityPlayAudio;
            EventHandler.OnSettingAudioSettings += EventHandler_OnSettingAudioSettings;
        }

        private void EventHandler_OnSettingAudioSettings(EventHandler source, EventHandler.EventHandlerArgs<AudioSettings> e)
        {
            Path = e.Event.Path;
        }

        private void EventHandler_OnUtilityPlayAudio(Shared.EventHandler source, Shared.EventHandler.EventHandlerArgs<Shared.Events.Utility.PlayAudio> e)
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
                EventBus.PublishEvent(new WriteToConsole() { Message = "Playing audio file failed: " + ex.Message });
            }
        }

        private void EventHandler_OnUtilitySay(Shared.EventHandler source, Shared.EventHandler.EventHandlerArgs<Shared.Events.Utility.Say> e)
        {
            if (e.Event == null || e.Event.Message == null || e.Event.Volume == null)
                return;

            Synthesizer.Volume = Math.Min((int)(100 * e.Event.Volume), 100);
            Synthesizer.Speak(e.Event.Message);
        }
    }
}
