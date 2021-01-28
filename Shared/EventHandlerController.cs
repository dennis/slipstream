using Slipstream.Shared.EventHandlers;
using System;
using System.Collections.Generic;
using static Slipstream.Shared.IEventHandlerController;

#nullable enable

namespace Slipstream.Shared
{
    public class EventHandlerController : IEventHandlerController
    {
        internal readonly IDictionary<dynamic, IEventHandler> Handlers = new Dictionary<dynamic, IEventHandler>();

        private volatile bool enabled = true;
        public bool Enabled { get { return enabled; } set { enabled = value; } }

        public event OnDefaultHandler? OnDefault;

        internal void Add(Type handlerInterface, IEventHandler implementation)
        {
            Handlers.Add(handlerInterface, implementation);
        }

        public T Get<T>()
        {
            return (T)Handlers[typeof(T)];
        }

        internal void Add(IEventHandler eventHandler)
        {
            Handlers.Add(eventHandler.GetType(), eventHandler);
        }

        public void HandleEvent(IEvent? ev)
        {
            if (ev == null || !Enabled)
                return;

            bool handled = false;

            foreach (var h in Handlers)
            {
                switch (h.Value.HandleEvent(ev))
                {
                    case IEventHandler.HandledStatus.Handled:
                        handled = true;
                        break;

                    case IEventHandler.HandledStatus.UseDefault:
                        OnDefault?.Invoke(this, new EventHandlerArgs<IEvent>(ev));
                        handled = true;
                        break;

                    case IEventHandler.HandledStatus.NotMine:
                        break;
                }

                if (handled)
                    break;
            }

            if (!handled)
            {
                throw new Exception($"Unknown event '{ev}");
            }
        }
    }
}