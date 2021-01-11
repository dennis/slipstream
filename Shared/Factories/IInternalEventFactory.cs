using Slipstream.Shared.Events.Internal;

#nullable enable

namespace Slipstream.Shared.Factories
{
    public interface IInternalEventFactory
    {
        public enum PluginStatusEnum
        {
            Registered, Unregistered
        }

        InternalCommandPluginRegister CreateInternalCommandPluginRegister(string pluginId, string pluginName);
        InternalCommandPluginStates CreateInternalCommandPluginStates();
        InternalCommandPluginUnregister CreateInternalCommandPluginUnregister(string pluginId);
        InternalPluginState CreateInternalPluginState(string pluginId, string pluginName, string displayName, PluginStatusEnum pluginStatus);
        InternalCommandReconfigure CreateInternalCommandReconfigure();
    }
}
