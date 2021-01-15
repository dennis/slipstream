#nullable enable

using Slipstream.Shared.Events.Audio;
using static Slipstream.Shared.EventHandler;

namespace Slipstream.Shared.EventHandlers
{
    internal class Audio : IEventHandler
    {
        private readonly EventHandler Parent;

        public Audio(EventHandler eventHandler)
        {
            Parent = eventHandler;
        }

        public delegate void OnAudioCommandPlayHandler(EventHandler source, EventHandlerArgs<AudioCommandPlay> e);

        public delegate void OnAudioCommandSayHandler(EventHandler source, EventHandlerArgs<AudioCommandSay> e);

        public delegate void OnAudioCommandSendDevicesHandler(EventHandler source, EventHandlerArgs<AudioCommandSendDevices> e);

        public delegate void OnAudioCommandSetOutputDeviceHandler(EventHandler source, EventHandlerArgs<AudioCommandSetOutputDevice> e);

        public delegate void OnAudioOutputDeviceHandler(EventHandler source, EventHandlerArgs<AudioOutputDevice> e);

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