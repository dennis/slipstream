#nullable enable

using Slipstream.Shared.Events.Lua;
using static Slipstream.Shared.EventHandler;

namespace Slipstream.Shared.EventHandlers
{
    internal class Lua : IEventHandler
    {
        private readonly EventHandler Parent;

        public Lua(EventHandler parent)
        {
            Parent = parent;
        }

        public delegate void OnLuaDeduplicateEventsHandler(EventHandler source, EventHandlerArgs<LuaCommandDeduplicateEvents> e);

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