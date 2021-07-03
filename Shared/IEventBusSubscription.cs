#nullable enable

namespace Slipstream.Shared
{
    public interface IEventBusSubscription : System.IDisposable
    {
        IEvent NextEvent();

        IEvent? NextEvent(int millisecondsTimeout);

        void AddImpersonate(string instanceId);

        void DeleteImpersonation(string instanceId);
    }
}