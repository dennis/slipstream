#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Speech.Synthesis;
using System.Threading;

using NAudio.Wave;

using Serilog;

using Slipstream.Components.Audio.Events;
using Slipstream.Shared;

namespace Slipstream.Components.Audio.Lua
{
#if WINDOWS

    public partial class AudioInstanceThread
    {
        private class AudioPlayerImpl
        {
            private readonly List<string> ActiveSenders = new List<string>();
            private readonly Queue<IEvent> AudioEvents = new Queue<IEvent>();
            private readonly AutoResetEvent AudioEventsSignal = new AutoResetEvent(false);
            private readonly Thread AudioPlayerThread;
            private readonly CancellationToken AudioPlayerThreadCancellationToken;
            private readonly string InstanceId;
            private readonly ILogger Logger;
            private readonly string Path;
            private readonly SpeechSynthesizer Synthesizer = new SpeechSynthesizer();
            private int OutputDeviceNumber = -1;

            public AudioPlayerImpl(string instanceId, ILogger logger, string path, int output, CancellationToken cancellationToken)
            {
                InstanceId = instanceId;
                AudioPlayerThreadCancellationToken = cancellationToken;
                Logger = logger;
                Path = path;
                OutputDeviceNumber = output;

                AudioPlayerThread = new Thread(new ThreadStart(AudioPlayerMain))
                {
                    Name = "AudioPlayerThread for " + InstanceId
                };
            }

            internal void AddAudioCommand(IEvent @event)
            {
                lock (AudioEvents)
                {
                    AudioEvents.Enqueue(@event);
                    AudioEventsSignal.Set();
                }
            }

            internal void AddSender(string sender)
            {
                lock (ActiveSenders)
                    ActiveSenders.Add(sender);
            }

            internal void RemoveSender(string sender)
            {
                lock (ActiveSenders)
                    ActiveSenders.Remove(sender);

                // Check if we got pending events for the removed instance. If so, remove them
                List<IEvent> events = new List<IEvent>();
                int removeEvents = 0;
                lock (AudioEvents)
                {
                    while (AudioEvents.TryDequeue(out IEvent? @event))
                    {
                        if (@event.Envelope.Sender != sender)
                            events.Add(@event);
                        else
                            removeEvents++;
                    }

                    // Rebuild queue
                    foreach (var @event in events)
                    {
                        AudioEvents.Enqueue(@event);
                    }
                    AudioEventsSignal.Set();
                }
            }

            internal void SetOutputDevice(int deviceIdx)
            {
                OutputDeviceNumber = deviceIdx;
            }

            internal void Start()
            {
                AudioPlayerThread.Start();
            }

            private void AudioPlayerMain()
            {
                while (!AudioPlayerThreadCancellationToken.IsCancellationRequested)
                {
                    if (AudioEventsSignal.WaitOne(100))
                    {
                        IEvent? @event = null;
                        do
                        {
                            lock (AudioEvents)
                                AudioEvents.TryDequeue(out @event);

                            if (@event != null)
                            {
                                if (@event is AudioCommandPlay play)
                                {
                                    OnAudioCommandPlay(play);
                                }
                                else if (@event is AudioCommandSay say)
                                {
                                    OnAudioCommandSay(say);
                                }
                            }
                        } while (@event != null);
                    }
                }
            }

            private void OnAudioCommandPlay(AudioCommandPlay @event)
            {
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
                using var stream = new MemoryStream();

                Synthesizer.SetOutputToWaveStream(stream);
                Synthesizer.Speak(@event.Message);

                stream.Flush();
                stream.Seek(0, SeekOrigin.Begin);

                Play(new WaveFileReader(stream), (float)@event.Volume);
            }

            private void Play(WaveStream stream, float volume)
            {
                using var outputDevice = new WaveOutEvent { DeviceNumber = OutputDeviceNumber };
                outputDevice.PlaybackStopped += PlaybackStoppedReceived;
                outputDevice.Init(stream);
                outputDevice.Volume = volume;
                outputDevice.Play();
                while (outputDevice.PlaybackState == PlaybackState.Playing && !AudioPlayerThreadCancellationToken.IsCancellationRequested)
                {
                    Thread.Sleep(100);
                }
            }

            private void PlaybackStoppedReceived(object? sender, StoppedEventArgs e)
            {
                if (e.Exception != null)
                {
                    Logger.Error("Error playing audio: {Message}", e.Exception.Message);
                }
            }
        }
    }

#endif
}