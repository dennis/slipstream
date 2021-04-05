using Slipstream.Shared;
using System;

namespace Slipstream.Backend
{
    public interface IEngine : IDisposable
    {
        void UnregisterSubscription(IEventBusSubscription subscription);

        void Start();
    }
}