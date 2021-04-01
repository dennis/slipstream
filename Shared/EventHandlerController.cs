using Autofac;
using Slipstream.Backend;
using System;
using System.Collections.Generic;
using static Slipstream.Shared.IEventHandlerController;

#nullable enable

namespace Slipstream.Shared
{
    public class EventHandlerController : IEventHandlerController
    {
        private readonly IDictionary<dynamic, IEventHandler> Handlers = new Dictionary<dynamic, IEventHandler>();

        private volatile bool enabled = true;
        public bool Enabled { get { return enabled; } set { enabled = value; } }

        public event EventHandler<IEvent>? OnDefault;

        public EventHandlerController(ILifetimeScope scope)
        {
            foreach (var h in scope.GetImplementingTypes<IEventHandler>())
            {
                Add((IEventHandler)scope.Resolve(h));
            }
        }

        public T Get<T>()
        {
            if (!Handlers.ContainsKey(typeof(T)))
                throw new KeyNotFoundException($"No EventHandler'{typeof(T)} found");

            return (T)Handlers[typeof(T)];
        }

        private void Add(IEventHandler eventHandler)
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
                        OnDefault?.Invoke(this, ev);
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