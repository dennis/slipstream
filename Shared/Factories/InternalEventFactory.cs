using Slipstream.Shared.Events.Internal;

#nullable enable

namespace Slipstream.Shared.Factories
{
    public class InternalEventFactory : IInternalEventFactory
    {
        public InternalCommandPluginRegister CreateInternalCommandPluginRegister(string pluginId, string pluginName)
        {
            return new InternalCommandPluginRegister { Id = pluginId, PluginName = pluginName };
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

        public InternalCommandReconfigure CreateInternalCommandReconfigure()
        {
            return new InternalCommandReconfigure();
        }
    }
}
