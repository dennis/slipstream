using Slipstream.Backend.Plugins;
using Slipstream.Backend.Services;
using Slipstream.Shared;
using System;

#nullable enable

namespace Slipstream.Backend
{
    public class PluginFactory
    {
        private readonly IEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private readonly IStateService StateService;
        private readonly ITxrxService TxrxService;
        private readonly IApplicationConfiguration ApplicationConfiguration;
        private readonly IPluginManager PluginManager;

        public PluginFactory(IEventFactory eventFactory, IEventBus eventBus, IStateService stateService, ITxrxService txrxService, IApplicationConfiguration applicationConfiguration, IPluginManager pluginManager)
        {
            EventFactory = eventFactory;
            EventBus = eventBus;
            StateService = stateService;
            TxrxService = txrxService;
            ApplicationConfiguration = applicationConfiguration;
            PluginManager = pluginManager;
        }

        public IPlugin CreatePlugin(string id, string name)
        {
            return name switch
            {
                "FileMonitorPlugin" => new FileMonitorPlugin(id, EventFactory, EventBus, ApplicationConfiguration),
                "FileTriggerPlugin" => new FileTriggerPlugin(id, EventFactory, EventBus, StateService, PluginManager),
                "AudioPlugin" => new AudioPlugin(id, EventFactory, EventBus, ApplicationConfiguration),
                "IRacingPlugin" => new IRacingPlugin(id, EventFactory, EventBus),
                "TwitchPlugin" => new TwitchPlugin(id, EventFactory, EventBus, ApplicationConfiguration),
                "TransmitterPlugin" => new TransmitterPlugin(id, EventFactory, EventBus, TxrxService, ApplicationConfiguration),
                "ReceiverPlugin" => new ReceiverPlugin(id, EventFactory, EventBus, TxrxService, ApplicationConfiguration),
                _ => throw new Exception($"Unknown plugin '{name}'"),
            };
        }
    }
}
