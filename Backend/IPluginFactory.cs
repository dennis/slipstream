#nullable enable

using Slipstream.Shared;
using System.Collections.Generic;

namespace Slipstream.Backend
{
    public interface IPluginFactory
    {
        IPlugin CreatePlugin(string id, string name);

        IPlugin CreatePlugin(string id, string name, IEventBus eventBus);

        IPlugin CreatePlugin(string pluginId, string name, Dictionary<string, dynamic> configuration);

        IPlugin CreatePlugin(string pluginId, string name, IEventBus eventBus, Dictionary<string, dynamic> configuration);
    }
}