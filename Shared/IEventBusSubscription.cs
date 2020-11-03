#nullable enable

namespace Slipstream.Shared
{
    public interface IEventBusSubscription
    {
        IEvent NextEvent();
        IEvent? NextEvent(int millisecondsTimeout);
    }
}
