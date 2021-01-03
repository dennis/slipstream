using Slipstream.Backend.Plugins;
using Slipstream.Backend.Services;
using Slipstream.Shared;
using Slipstream.Shared.Events.Setting;
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

        public PluginFactory(IEventFactory eventFactory, IEventBus eventBus, IStateService stateService, ITxrxService txrxService)
        {
            EventFactory = eventFactory;
            EventBus = eventBus;
            StateService = stateService;
            TxrxService = txrxService;
        }

        public IPlugin CreatePlugin(string id, string name, IEvent? settings)
        {
            switch (name)
            {
                case "FileMonitorPlugin":
                    {
                        if (!(settings is FileMonitorSettings typedSettings))
                        {
                            throw new Exception("Unexpected settings for FileMonitorPlugin");
                        }
                        else
                        {
                            return new FileMonitorPlugin(id, EventFactory, EventBus, typedSettings);
                        }
                    }
                case "FileTriggerPlugin":
                    return new FileTriggerPlugin(id, EventFactory, EventBus);
                case "LuaPlugin":
                    {
                        if (!(settings is LuaSettings typedSettings))
                        {
                            throw new Exception("Unexpected settings for LuaPlugin");
                        }
                        else
                        {
                            return new LuaPlugin(id, EventFactory, EventBus, StateService, typedSettings);
                        }
                    }
                case "AudioPlugin":
                    {
                        if (!(settings is AudioSettings typedSettings))
                        {
                            throw new Exception("Unexpected settings for AudioPlugin");
                        }
                        else
                        {
                            return new AudioPlugin(id, EventFactory, EventBus, typedSettings);
                        }
                    }
                case "IRacingPlugin":
                    return new IRacingPlugin(id, EventFactory, EventBus);
                case "TwitchPlugin":
                    {
                        if (!(settings is TwitchSettings typedSettings))
                        {
                            throw new Exception("Unexpected settings for TwitchPlugin");
                        }
                        else
                        {
                            return new TwitchPlugin(id, EventFactory, EventBus, typedSettings);
                        }
                    }
                case "TransmitterPlugin":
                    {
                        if (!(settings is TxrxSettings typedSettings))
                        {
                            throw new Exception("Unexpected settings for TransmitterPlugin");
                        }
                        else
                        {
                            return new TransmitterPlugin(id, EventFactory, EventBus, TxrxService, typedSettings);
                        }
                    }
                case "ReceiverPlugin":
                    {
                        if (!(settings is TxrxSettings typedSettings))
                        {
                            throw new Exception("Unexpected settings for ReceiverPlugin");
                        }
                        else
                        {
                            return new ReceiverPlugin(id, EventFactory, EventBus, TxrxService, typedSettings);
                        }
                    }
                default:
                    throw new Exception($"Unknown plugin '{name}'");
            }
        }
    }
}
