using Slipstream.Backend;

namespace Slipstream.Shared
{
    public interface IEventBus : IEventProducer
    {
        IEventBusSubscription RegisterListener();
        void UnregisterSubscription(IEventBusSubscription eventBusSubscription);
    }
}
