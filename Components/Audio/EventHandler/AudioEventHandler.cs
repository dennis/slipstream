#nullable enable

using Slipstream.Components.Audio.Events;
using Slipstream.Shared;

namespace Slipstream.Components.Audio.EventHandler
{
    internal class AudioEventHandler : IEventHandler
    {
        private readonly EventHandlerController Parent;

        public AudioEventHandler(EventHandlerController eventHandler)
        {
            Parent = eventHandler;
        }

        public delegate void OnAudioCommandPlayHandler(EventHandlerController source, EventHandlerArgs<AudioCommandPlay> e);

        public delegate void OnAudioCommandSayHandler(EventHandlerController source, EventHandlerArgs<AudioCommandSay> e);

        public delegate void OnAudioCommandSendDevicesHandler(EventHandlerController source, EventHandlerArgs<AudioCommandSendDevices> e);

        public delegate void OnAudioCommandSetOutputDeviceHandler(EventHandlerController source, EventHandlerArgs<AudioCommandSetOutputDevice> e);

        public delegate void OnAudioOutputDeviceHandler(EventHandlerController source, EventHandlerArgs<AudioOutputDevice> e);

        public event OnAudioCommandPlayHandler? OnAudioCommandPlay;

        public event OnAudioCommandSayHandler? OnAudioCommandSay;

        public event OnAudioCommandSendDevicesHandler? OnAudioCommandSendDevices;

        public event OnAudioCommandSetOutputDeviceHandler? OnAudioCommandSetOutputDevice;

        public event OnAudioOutputDeviceHandler? OnAudioOutputDevice;

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            switch (@event)
            {
                case AudioCommandSay tev:
                    if (OnAudioCommandSay != null)
                    {
                        OnAudioCommandSay.Invoke(Parent, new EventHandlerArgs<AudioCommandSay>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case AudioCommandPlay tev:
                    if (OnAudioCommandPlay != null)
                    {
                        OnAudioCommandPlay.Invoke(Parent, new EventHandlerArgs<AudioCommandPlay>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case AudioCommandSendDevices tev:
                    if (OnAudioCommandSendDevices != null)
                    {
                        OnAudioCommandSendDevices.Invoke(Parent, new EventHandlerArgs<AudioCommandSendDevices>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case AudioOutputDevice tev:
                    if (OnAudioOutputDevice != null)
                    {
                        OnAudioOutputDevice.Invoke(Parent, new EventHandlerArgs<AudioOutputDevice>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case AudioCommandSetOutputDevice tev:
                    if (OnAudioCommandSetOutputDevice != null)
                    {
                        OnAudioCommandSetOutputDevice.Invoke(Parent, new EventHandlerArgs<AudioCommandSetOutputDevice>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
            }

            return IEventHandler.HandledStatus.NotMine;
        }
    }
}