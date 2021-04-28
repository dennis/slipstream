#nullable enable

using System;
using System.IO;
using System.Speech.Synthesis;
using System.Threading;
using NAudio.Wave;
using Serilog;
using Slipstream.Components.Audio.EventHandler;
using Slipstream.Components.Audio.Events;
using Slipstream.Shared;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.Audio.Lua
{
    public class AudioInstanceThread : BaseInstanceThread, IAudioInstanceThread, IDisposable
    {
        private readonly SpeechSynthesizer Synthesizer = new SpeechSynthesizer();
        private readonly string Path;
        private readonly IEventHandlerController EventHandlerController;
        private readonly IEventBusSubscription Subscription;
        private readonly IAudioEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private WaveOutEvent OutputDevice = new WaveOutEvent();

        public AudioInstanceThread(string instanceId, int output, string path, IEventBusSubscription eventBusSubscription, IEventHandlerController eventHandlerController, IEventBus eventBus, IAudioEventFactory audioEventFactory, ILogger logger) : base(instanceId, logger)
        {
            Path = path;
            Subscription = eventBusSubscription;
            EventHandlerController = eventHandlerController;
            EventFactory = audioEventFactory;
            EventBus = eventBus;

            if (output == -1)
            {
                Synthesizer.SetOutputToDefaultAudioDevice();
            }
            else
            {
                OutputDevice = new WaveOutEvent() { DeviceNumber = output };
            }
        }

        protected override void Main()
        {
            var audioEventHandler = EventHandlerController.Get<AudioEventHandler>();
            audioEventHandler.OnAudioCommandSay += (_, e) => OnAudioCommandSay(e);
            audioEventHandler.OnAudioCommandPlay += (_, e) => OnAudioCommandPlay(e);
            audioEventHandler.OnAudioCommandSendDevices += (_, e) => OnAudioCommandSendDevices(e);
            audioEventHandler.OnAudioCommandSetOutputDevice += (_, e) => OnAudioCommandSetOutputDevice(e);

            while (!Stopping)
            {
                IEvent? @event = Subscription.NextEvent(100);

                if (@event != null)
                {
                    EventHandlerController.HandleEvent(@event);
                }
            }
        }

        private void OnAudioCommandSetOutputDevice(AudioCommandSetOutputDevice @event)
        {
            if (@event.InstanceId != InstanceId)
                return;

            OutputDevice = new WaveOutEvent() { DeviceNumber = @event.DeviceIdx };
        }

        private void OnAudioCommandSendDevices(AudioCommandSendDevices @event)
        {
            if (@event.InstanceId != InstanceId)
                return;

            for (int n = -1; n < WaveOut.DeviceCount; n++)
            {
                var caps = WaveOut.GetCapabilities(n);

                EventBus.PublishEvent(EventFactory.CreateAudioOutputDevice(InstanceId, caps.ProductName, n));
            }
        }

        private void OnAudioCommandPlay(AudioCommandPlay @event)
        {
            if (@event.InstanceId != InstanceId)
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
            if (@event.InstanceId != InstanceId)
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
            while (OutputDevice.PlaybackState == PlaybackState.Playing && !Stopping)
            {
                Thread.Sleep(100);
            }
        }
    }
}