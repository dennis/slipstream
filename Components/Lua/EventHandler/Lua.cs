#nullable enable

using Slipstream.Components.Lua.Events;
using Slipstream.Shared;
using System;

namespace Slipstream.Components.Lua.EventHandler
{
    internal class Lua : IEventHandler
    {
        private readonly EventHandlerController Parent;

        public Lua(EventHandlerController parent)
        {
            Parent = parent;
        }

        public event EventHandler<LuaCommandDeduplicateEvents>? OnLuaCommandDeduplicateEvents;

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            return @event switch
            {
                LuaCommandDeduplicateEvents tev => OnEvent(OnLuaCommandDeduplicateEvents, tev),
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