using Slipstream.Components.Internal.Events;
using Slipstream.Shared.Helpers.StrongParameters;

#nullable enable

namespace Slipstream.Components.Internal
{
    public interface IInternalEventFactory
    {
        public enum PluginStatusEnum
        {
            Registered, Unregistered
        }

        InternalCommandPluginRegister CreateInternalCommandPluginRegister(string pluginId, string pluginName);

        InternalCommandPluginRegister CreateInternalCommandPluginRegister(string pluginId, string pluginName, Parameters configuration);

        InternalCommandPluginStates CreateInternalCommandPluginStates();

        InternalCommandPluginUnregister CreateInternalCommandPluginUnregister(string pluginId);

        InternalPluginState CreateInternalPluginState(string pluginId, string pluginName, string displayName, PluginStatusEnum pluginStatus);

        InternalShutdown CreateInternalShutdown();

        InternalCommandShutdown CreateInternalCommandShutdown();
    }
}