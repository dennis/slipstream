#nullable enable

using Slipstream.Shared.EventHandlers;
using System;
using System.Collections.Generic;

namespace Slipstream.Shared
{
    internal interface IEventHandler
    {
        internal enum HandledStatus
        {
            NotMine, Handled, UseDefault
        }

        HandledStatus HandleEvent(IEvent @event);
    }

    public class EventHandler
    {
        public class EventHandlerArgs<T> : EventArgs
        {
            public T Event { get; }

            public EventHandlerArgs(T e)
            {
                Event = e;
            }
        }

        internal readonly IDictionary<dynamic, IEventHandler> Handlers = new Dictionary<dynamic, IEventHandler>();

        private volatile bool enabled = true;
        public bool Enabled { get { return enabled; } set { enabled = value; } }

        public EventHandler()
        {
            Handlers.Add(typeof(Internal), new Internal(this));
            Handlers.Add(typeof(Lua), new Lua(this));
            Handlers.Add(typeof(UI), new UI(this));
            Handlers.Add(typeof(Audio), new Audio(this));
            Handlers.Add(typeof(IRacing), new IRacing(this));
            Handlers.Add(typeof(Twitch), new Twitch(this));
            Handlers.Add(typeof(FileMonitor), new FileMonitor(this));
            Handlers.Add(typeof(Playback), new Playback(this));
        }

        public delegate void OnDefaultHandler(EventHandler source, EventHandlerArgs<IEvent> e);

        public event OnDefaultHandler? OnDefault;

        public T Get<T>()
        {
            return (T)Handlers[typeof(T)];
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