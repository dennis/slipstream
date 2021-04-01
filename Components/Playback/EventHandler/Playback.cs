#nullable enable

using Slipstream.Components.Playback.Events;
using Slipstream.Shared;
using System;

namespace Slipstream.Components.Playback.EventHandler
{
    internal class Playback : IEventHandler
    {
        private readonly EventHandlerController Parent;

        public Playback(EventHandlerController parent)
        {
            Parent = parent;
        }

        public event EventHandler<PlaybackCommandInjectEvents>? OnPlaybackCommandInjectEvents;

        public event EventHandler<PlaybackCommandSaveEvents>? OnPlaybackCommandSaveEvents;

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            return @event switch
            {
                PlaybackCommandInjectEvents tev => OnEvent(OnPlaybackCommandInjectEvents, tev),
                PlaybackCommandSaveEvents tev => OnEvent(OnPlaybackCommandSaveEvents, tev),
                _ => IEventHandler.HandledStatus.NotMine,
            };
        }

        private IEventHandler.HandledStatus OnEvent<TEvent>(EventHandler<TEvent>? onEvent, TEvent args)
        {
            if (onEvent != null)
            {
                onEvent.Invoke(Parent, args);
                return IEventHandler.HandledStatus.Handled;
            }
            return IEventHandler.HandledStatus.UseDefault;
        }
    }
}