using System;

#nullable enable

namespace Slipstream.Backend
{
    public interface IPlugin : IDisposable
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
        public string WorkerName { get; }
        public Shared.EventHandler EventHandler { get; }
        public bool Reconfigurable { get; }
        void Loop();
    }
}
