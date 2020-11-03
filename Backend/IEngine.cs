using Slipstream.Shared;

namespace Slipstream.Backend
{
    interface IEngine
    {
        IEventBusSubscription RegisterListener(IEventListener listener);
        void UnregisterListener(IEventListener listener);
        void Start();
        void Stop();
        void Dispose();
    }
}
