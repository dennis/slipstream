﻿using NAudio.Wave;
using Slipstream.Shared;
using Slipstream.Shared.Events.Setting;
using Slipstream.Shared.Events.Utility;
using System;
using System.Diagnostics;
using System.Speech.Synthesis;
using System.Threading;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    class AudioPlugin : Worker, IPlugin
    {
        public Guid Id { get; set; }
        public string Name => "AudioPlugin";
        public string DisplayName => Name;
        public bool Enabled { get; internal set; }

        private readonly IEventBus EventBus;
        private IEventBusSubscription? Subscription;
        private string? Path;
        private readonly SpeechSynthesizer Synthesizer;

        public AudioPlugin(IEvent settings, IEventBus eventBus)
        {
            Id = Guid.Parse("ee6c135a-9b61-4674-8f5b-9f61ecd15f2c");
            EventBus = eventBus;

            if (settings is AudioSettings typedSettings)
                OnAudioSettings(typedSettings);
            else
                throw new System.Exception($"Unexpected event as Exception {settings}");

            Synthesizer = new SpeechSynthesizer();
            Synthesizer.SetOutputToDefaultAudioDevice();
        }

        private void OnAudioSettings(AudioSettings typedSettings)
        {
            Path = typedSettings.Path;
        }

        public void Disable(IEngine engine)
        {
            Enabled = false;
        }

        public void Enable(IEngine engine)
        {
            Enabled = true;
        }

        public void RegisterPlugin(IEngine engine)
        {
            Subscription = engine.RegisterListener();
            Start();
        }

        public void UnregisterPlugin(IEngine engine)
        {
            Subscription?.Dispose();
            Subscription = null;
            Stop();
        }
        protected override void Main()
        {
            Shared.EventHandler eventHandler = new Shared.EventHandler();

            eventHandler.OnUtilitySay += EventHandler_OnUtilitySay;
            eventHandler.OnPlayAudio += EventHandler_OnPlayAudio;

            while (!Stopped)
            {
                var e = Subscription?.NextEvent(250);

                if (Enabled)
                {
                    eventHandler.HandleEvent(e);
                }
            }
        }

        private void EventHandler_OnPlayAudio(Shared.EventHandler source, Shared.EventHandler.EventHandlerArgs<Shared.Events.Utility.PlayAudio> e)
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