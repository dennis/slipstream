using Slipstream.Shared;

namespace Slipstream.Backend
{
    interface IEngine
    {
        IEventBusSubscription RegisterListener();
        void UnregisterSubscription(IEventBusSubscription subscription);
        void Start();
        void Stop();
        void Dispose();
    }
}
