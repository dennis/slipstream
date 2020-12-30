using Slipstream.Shared;

namespace Slipstream.Backend
{
    public interface IEngine
    {
        IEventBusSubscription RegisterListener();
        void UnregisterSubscription(IEventBusSubscription subscription);
        void Start();
        void Dispose();
    }
}
