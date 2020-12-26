namespace Slipstream.Shared
{
    public interface IEventBus : IEventProducer
    {
        public bool Enabled { get; set; }
        IEventBusSubscription RegisterListener();
        void UnregisterSubscription(IEventBusSubscription eventBusSubscription);
    }
}
