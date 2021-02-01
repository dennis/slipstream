using System;

namespace Slipstream.Shared
{
    public interface IEventFactory
    {
        T Get<T>();

        void Add<T>(Type factoryInterface, T factoryImplementation);
    }
}