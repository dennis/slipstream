#nullable enable

using Slipstream.Shared.Events.LuaManager;
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

        public delegate void OnInternalCommandDeduplicateEventsHandler(EventHandler source, EventHandlerArgs<LuaManagerCommandDeduplicateEvents> e);

        public event OnInternalCommandDeduplicateEventsHandler? OnLuaManagerCommandDeduplicateEvents;

        public IEventHandler.HandledStatus HandleEvent(IEvent @event)
        {
            switch (@event)
            {
                case LuaManagerCommandDeduplicateEvents tev:
                    if (OnLuaManagerCommandDeduplicateEvents != null)
                    {
                        OnLuaManagerCommandDeduplicateEvents?.Invoke(Parent, new EventHandlerArgs<LuaManagerCommandDeduplicateEvents>(tev));
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