#nullable enable

namespace Slipstream.Components.WebWidget
{
    public interface IHttpServerApi
    {
        // These needs to be threadsafe!
        void AddInstance(string instanceId, string instanceType, string? data);

        void RemoveInstance(string instanceId);

        bool ContainsInstance(string instanceId);
    }
}