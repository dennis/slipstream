#nullable enable

using System;

namespace Slipstream.Shared
{
    public class EventHandlerArgs<T> : EventArgs
    {
        public T Event { get; }

        public EventHandlerArgs(T e)
        {
            Event = e;
        }
    }
}