using AutoUpdaterDotNET;

namespace Slipstream.Shared
{
    public interface IEventBus : IEventProducer
    {
        public bool Enabled { get; set; }

        IEventBusSubscription RegisterListener(string instanceId, bool fromBeginning = false, bool promiscuousMode = false);

        void UnregisterSubscription(IEventBusSubscription eventBusSubscription);
    }
}