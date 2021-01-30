#nullable enable

using Slipstream.Components.Playback.Events;
using Slipstream.Shared;

namespace Slipstream.Components.Playback.EventHandler
{
    internal class Playback : IEventHandler
    {
        private readonly EventHandlerController Parent;

        public Playback(EventHandlerController parent)
        {
            Parent = parent;
        }

        public delegate void OnPlaybackCommandInjectEventsHandler(EventHandlerController source, EventHandlerArgs<PlaybackCommandInjectEvents> e);

        public event OnPlaybackCommandInjectEventsHandler? OnPlaybackCommandInjectEvents;

        public delegate void OnPlaybackCommandSaveEventsHandler(EventHandlerController source, EventHandlerArgs<PlaybackCommandSaveEvents> e);

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