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

        public event OnAudioCommandPlayHandler? OnAudioCommandPlay;

        public event OnAudioCommandSayHandler? OnAudioCommandSay;

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
            }

            return IEventHandler.HandledStatus.NotMine;
        }
    }
}