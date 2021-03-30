using Slipstream.Shared;

namespace Slipstream.Backend
{
    public interface IEngine
    {
        void UnregisterSubscription(IEventBusSubscription subscription);

        void Start();

        void Dispose();
    }
}