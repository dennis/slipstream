#nullable enable


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
}