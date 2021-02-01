#nullable enable

using Slipstream.Components.Lua.Events;
using Slipstream.Shared;

namespace Slipstream.Components.Lua.EventHandler
{
    internal class Lua : IEventHandler
    {
        private readonly EventHandlerController Parent;

        public Lua(EventHandlerController parent)
        {
            Parent = parent;
        }

        public delegate void OnLuaDeduplicateEventsHandler(EventHandlerController source, EventHandlerArgs<LuaCommandDeduplicateEvents> e);

        public event OnLuaDeduplicateEventsHandler? OnLuaCommandDeduplicateEvents;

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            switch (@event)
            {
                case LuaCommandDeduplicateEvents tev:
                    if (OnLuaCommandDeduplicateEvents != null)
                    {
                        OnLuaCommandDeduplicateEvents?.Invoke(Parent, new EventHandlerArgs<LuaCommandDeduplicateEvents>(tev));
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