using Slipstream.Shared;
using System;
using static Slipstream.Shared.EventHandler;

#nullable enable

namespace Slipstream.Backend
{
    public interface IPlugin
    {
        public class EventHandlerArgs<T> : EventArgs
        {
            public T Event { get; }

            public EventHandlerArgs(T e)
            {
                Event = e;
            }
        }

        public delegate void OnStateChangedHandler(IPlugin source, EventHandlerArgs<IPlugin> e);
        public event OnStateChangedHandler? OnStateChanged;
        public string Id { get; }
        public string Name { get; }
        public string DisplayName { get; }
        public bool Enabled { get; }
        public string WorkerName { get; }
        public Shared.EventHandler EventHandler { get; }
        void RegisterPlugin(IEngine engine);
        void UnregisterPlugin(IEngine engine);
        void Enable(IEngine engine);
        void Disable(IEngine engine);
        void Loop();
    }
}
