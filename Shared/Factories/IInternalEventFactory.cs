using Slipstream.Shared.Events.Internal;
using System.Collections.Generic;

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

        InternalCommandPluginRegister CreateInternalCommandPluginRegister(string pluginId, string pluginName, Dictionary<string, dynamic> configuration);

        InternalCommandPluginStates CreateInternalCommandPluginStates();

        InternalCommandPluginUnregister CreateInternalCommandPluginUnregister(string pluginId);

        InternalPluginState CreateInternalPluginState(string pluginId, string pluginName, string displayName, PluginStatusEnum pluginStatus);

        InternalCommandReconfigure CreateInternalCommandReconfigure();
    }
}