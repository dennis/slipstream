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

        public delegate void OnPlaybackInjectEventsHandler(EventHandler source, EventHandlerArgs<PlaybackInjectEvents> e);
        public event OnPlaybackInjectEventsHandler? OnPlaybackInjectEvents;
        public delegate void OnPlaybackSaveEventsHandler(EventHandler source, EventHandlerArgs<PlaybackSaveEvents> e);
        public event OnPlaybackSaveEventsHandler? OnPlaybackSaveEvents;

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            switch (@event)
            {
                case PlaybackInjectEvents tev:
                    if (OnPlaybackInjectEvents != null)
                    {
                        OnPlaybackInjectEvents.Invoke(Parent, new EventHandlerArgs<PlaybackInjectEvents>(tev));
                        return IEventHandler.HandledStatus.Handled;
                    }
                    else
                    {
                        return IEventHandler.HandledStatus.UseDefault;
                    }
                case PlaybackSaveEvents tev:
                    if (OnPlaybackSaveEvents != null)
                    {
                        OnPlaybackSaveEvents.Invoke(Parent, new EventHandlerArgs<PlaybackSaveEvents>(tev));
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