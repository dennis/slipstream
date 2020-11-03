namespace Slipstream.Shared
{
    public interface IEventBus : IEventProducer
    {
        IEventBusSubscription RegisterListener(IEventListener listener);
        IEventBusSubscription RegisterListener(string listenerName);
        void UnregisterListener(IEventListener listener);
        void UnregisterListener(string listenerName);
    }
}
