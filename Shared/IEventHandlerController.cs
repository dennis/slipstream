#nullable enable

namespace Slipstream.Shared
{
    public interface IEventHandlerController
    {
        public bool Enabled { get; set; }

        public delegate void OnDefaultHandler(IEventHandlerController source, EventHandlerArgs<IEvent> e);

        public event OnDefaultHandler? OnDefault;

        public T Get<T>();

        public void HandleEvent(IEvent? ev);
    }
}