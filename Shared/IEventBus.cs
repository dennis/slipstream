namespace Slipstream.Shared
{
    public interface IEventBus : IEventProducer
    {
        public bool Enabled { get; set; }
        IEventBusSubscription RegisterListener(bool fromBeginning = false);
        void UnregisterSubscription(IEventBusSubscription eventBusSubscription);
    }
}
