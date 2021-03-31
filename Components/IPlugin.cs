using Slipstream.Shared;
using System;

#nullable enable

namespace Slipstream.Components
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
        public IEventHandlerController EventHandlerController { get; }
        public bool Reconfigurable { get; }
        public bool FullThreadControl { get; }

        public void Run();
    }
}