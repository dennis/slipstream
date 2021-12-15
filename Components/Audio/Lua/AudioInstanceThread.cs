#nullable enable

using System;
using System.Runtime.Versioning;
using System.Threading;

using NAudio.Wave;

using Serilog;

using Slipstream.Components.Audio.EventHandler;
using Slipstream.Components.Audio.Events;
using Slipstream.Components.Internal;
using Slipstream.Components.Internal.Events;
using Slipstream.Shared;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.Audio.Lua
{
#if WINDOWS

    [SupportedOSPlatform("windows")]
    public partial class AudioInstanceThread : BaseInstanceThread, IAudioInstanceThread, IDisposable
    {
        private readonly IEventHandlerController EventHandlerController;
        private readonly IEventBusSubscription Subscription;
        private readonly IAudioEventFactory EventFactory;
        private readonly CancellationTokenSource AudioPlayerThreadCts = new CancellationTokenSource();
        private readonly AudioPlayerImpl AudioPlayer;

        public AudioInstanceThread(
            string luaLibraryName,
            string instanceId,
            int output,
            string path,
            IEventBusSubscription eventBusSubscription,
            IEventHandlerController eventHandlerController,
            IEventBus eventBus,
            IAudioEventFactory audioEventFactory,
            IInternalEventFactory internalEventFactory,
            ILogger logger) : base(luaLibraryName, instanceId, logger, eventHandlerController, eventBus, internalEventFactory)
        {
            Subscription = eventBusSubscription;
            EventHandlerController = eventHandlerController;
            EventFactory = audioEventFactory;
            EventBus = eventBus;

            AudioPlayer = new AudioPlayerImpl(instanceId, logger, path, output, AudioPlayerThreadCts.Token);
            AudioPlayer.Start();
        }

        protected override void Main()
        {
            var internalEventHandler = EventHandlerController.Get<Internal.EventHandler.Internal>();
            internalEventHandler.OnInternalDependencyAdded += (_, e) => OnInternalDependencyAdded(e);
            internalEventHandler.OnInternalDependencyRemoved += (_, e) => OnInternalDependencyRemoved(e);

            var audioEventHandler = EventHandlerController.Get<AudioEventHandler>();
            audioEventHandler.OnAudioCommandSay += (_, e) => AudioPlayer.AddAudioCommand(e);
            audioEventHandler.OnAudioCommandPlay += (_, e) => AudioPlayer.AddAudioCommand(e);
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

            Logger.Debug("Cancelling AudioPlayer for {InstanceId}", InstanceId);
            AudioPlayerThreadCts.Cancel();
        }

        private void OnInternalDependencyRemoved(InternalDependencyRemoved e)
        {
            if (e.DependsOn != InstanceId)
                return;

            AudioPlayer.RemoveSender(e.Envelope.Sender);
        }

        private void OnInternalDependencyAdded(InternalDependencyAdded e)
        {
            if (e.DependsOn != InstanceId)
                return;

            AudioPlayer.AddSender(e.Envelope.Sender);
        }

        private void OnAudioCommandSetOutputDevice(AudioCommandSetOutputDevice @event)
        {
            AudioPlayer.SetOutputDevice(@event.DeviceIdx);
        }

        private void OnAudioCommandSendDevices(AudioCommandSendDevices _)
        {
            for (int n = -1; n < WaveOut.DeviceCount; n++)
            {
                var caps = WaveOut.GetCapabilities(n);

                EventBus.PublishEvent(EventFactory.CreateAudioOutputDevice(InstanceEnvelope, caps.ProductName, n));
            }
        }
    }

#endif
}