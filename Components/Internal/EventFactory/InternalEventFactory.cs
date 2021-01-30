using Newtonsoft.Json;
using Slipstream.Components.Internal.Events;
using Slipstream.Shared.Helpers.StrongParameters;

#nullable enable

namespace Slipstream.Components.Internal.EventFactory
{
    public class InternalEventFactory : IInternalEventFactory
    {
        public InternalCommandPluginRegister CreateInternalCommandPluginRegister(string pluginId, string pluginName)
        {
            return new InternalCommandPluginRegister { Id = pluginId, PluginName = pluginName, Configuration = "{}" };
        }

        public InternalCommandPluginRegister CreateInternalCommandPluginRegister(string pluginId, string pluginName, Parameters configuration)
        {
            var jsonConfig = JsonConvert.SerializeObject(configuration);

            return new InternalCommandPluginRegister { Id = pluginId, PluginName = pluginName, Configuration = jsonConfig };
        }

        public InternalCommandPluginStates CreateInternalCommandPluginStates()
        {
            return new InternalCommandPluginStates();
        }

        public InternalCommandPluginUnregister CreateInternalCommandPluginUnregister(string pluginId)
        {
            return new InternalCommandPluginUnregister { Id = pluginId };
        }

        public InternalPluginState CreateInternalPluginState(string pluginId, string pluginName, string displayName, IInternalEventFactory.PluginStatusEnum pluginStatus)
        {
            return new InternalPluginState { Id = pluginId, PluginName = pluginName, DisplayName = displayName, PluginStatus = pluginStatus.ToString() };
        }
    }
}