#nullable enable

using Slipstream.Components.Audio.Events;
using Slipstream.Shared;
using System;

namespace Slipstream.Components.Audio.EventHandler
{
    internal class AudioEventHandler : IEventHandler
    {
        public event EventHandler<AudioCommandPlay>? OnAudioCommandPlay;

        public event EventHandler<AudioCommandSay>? OnAudioCommandSay;

        public event EventHandler<AudioCommandSendDevices>? OnAudioCommandSendDevices;

        public event EventHandler<AudioCommandSetOutputDevice>? OnAudioCommandSetOutputDevice;

        public event EventHandler<AudioOutputDevice>? OnAudioOutputDevice;

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            return @event switch
            {
                AudioCommandSay tev => OnEvent(OnAudioCommandSay, tev),
                AudioCommandPlay tev => OnEvent(OnAudioCommandPlay, tev),
                AudioCommandSendDevices tev => OnEvent(OnAudioCommandSendDevices, tev),
                AudioOutputDevice tev => OnEvent(OnAudioOutputDevice, tev),
                AudioCommandSetOutputDevice tev => OnEvent(OnAudioCommandSetOutputDevice, tev),
                _ => IEventHandler.HandledStatus.NotMine,
            };
        }

        private IEventHandler.HandledStatus OnEvent<TEvent>(EventHandler<TEvent>? onEvent, TEvent args)
        {
            if (onEvent != null)
            {
                onEvent.Invoke(this, args);
                return IEventHandler.HandledStatus.Handled;
            }
            return IEventHandler.HandledStatus.UseDefault;
        }
    }
}