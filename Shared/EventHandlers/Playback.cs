#nullable enable

using Slipstream.Shared.Events.Playback;
using static Slipstream.Shared.EventHandler;

namespace Slipstream.Shared.EventHandlers
{
    internal class Playback : IEventHandler
    {
        private readonly EventHandler Parent;

        public Playback(EventHandler parent)
        {
            Parent = parent;
        }

        public delegate void OnPlaybackCommandInjectEventsHandler(EventHandler source, EventHandlerArgs<PlaybackCommandInjectEvents> e);
        public event OnPlaybackCommandInjectEventsHandler? OnPlaybackCommandInjectEvents;
        public delegate void OnPlaybackCommandSaveEventsHandler(EventHandler source, EventHandlerArgs<PlaybackCommandSaveEvents> e);
        public event OnPlaybackCommandSaveEventsHandler? OnPlaybackCommandSaveEvents;

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            switch (@event)
            {
                case PlaybackCommandInjectEvents tev:
                    if (OnPlaybackCommandInjectEvents != null)
                    {
                        OnPlaybackCommandInjectEvents.Invoke(Parent, new EventHandlerArgs<PlaybackCommandInjectEvents>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case PlaybackCommandSaveEvents tev:
                    if (OnPlaybackCommandSaveEvents != null)
                    {
                        OnPlaybackCommandSaveEvents.Invoke(Parent, new EventHandlerArgs<PlaybackCommandSaveEvents>(tev));
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