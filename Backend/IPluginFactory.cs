#nullable enable

using Slipstream.Shared;

namespace Slipstream.Backend
{
    public interface IPluginFactory
    {
        IPlugin CreatePlugin(string id, string name);
        IPlugin CreatePlugin(string id, string name, IEventBus eventBus);
        IPlugin CreatePlugin<T>(string pluginId, string name, T configuration);
        IPlugin CreatePlugin<T>(string pluginId, string name, IEventBus eventBus, T configuration);
    }
}
