using NAudio.Wave;
using Slipstream.Shared;
using Slipstream.Shared.Events.Setting;
using Slipstream.Shared.Events.Utility;
using System;
using System.Diagnostics;
using System.Speech.Synthesis;
using System.Threading;
using EventHandler = Slipstream.Shared.EventHandler;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    class AudioPlugin : IPlugin
    {
        public string Id { get; }
        public string Name => "AudioPlugin";
        public string DisplayName => Name;
        public bool Enabled { get; internal set; }
        public string WorkerName => "Core";
        public EventHandler EventHandler { get; } = new EventHandler();

        private readonly IEventBus EventBus;
        private string? Path;
        private readonly SpeechSynthesizer Synthesizer;

        public AudioPlugin(string id, IEvent settings, IEventBus eventBus)
        {
            Id = id;
            EventBus = eventBus;

            if (settings is AudioSettings typedSettings)
                OnAudioSettings(typedSettings);
            else
                throw new System.Exception($"Unexpected event as Exception {settings}");

            Synthesizer = new SpeechSynthesizer();
            Synthesizer.SetOutputToDefaultAudioDevice();

            EventHandler.OnUtilitySay += EventHandler_OnUtilitySay;
            EventHandler.OnUtilityPlayAudio += EventHandler_OnUtilityPlayAudio;
        }

        private void OnAudioSettings(AudioSettings typedSettings)
        {
            Path = typedSettings.Path;
        }

        public void Disable(IEngine engine)
        {
        }

        public void Enable(IEngine engine)
        {
        }

        public void RegisterPlugin(IEngine engine)
        {
        }

        public void UnregisterPlugin(IEngine engine)
        {
        }

        public void Loop()
        {
        }

        private void EventHandler_OnUtilityPlayAudio(Shared.EventHandler source, Shared.EventHandler.EventHandlerArgs<Shared.Events.Utility.PlayAudio> e)
        {
            var filename = e.Event.Filename;
            var volume = e.Event.Volume;

            if (filename == null || volume == null)
                return;

            var filePath = System.IO.Path.Combine(Path, filename);

            Debug.WriteLine($"Playing {filePath}");

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
