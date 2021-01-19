#nullable enable

using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;

namespace Slipstream.Backend
{
    public interface IPluginFactory
    {
        IPlugin CreatePlugin(string pluginId, string name, Parameters configuration);

        IPlugin CreatePlugin(string pluginId, string name, IEventBus eventBus, Parameters configuration);
    }
}