using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using Slipstream.Backend.Services;
using Slipstream.Shared;
using Slipstream.Shared.Events.Internal;
using Slipstream.Shared.Factories;
using Slipstream.Shared.Helpers.StrongParameters;
using System;
using System.Collections.Generic;
using System.IO;

#nullable enable

namespace Slipstream.Backend
{
    internal class Engine : Worker, IEngine, IDisposable
    {
        private readonly IInternalEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private readonly IPluginManager PluginManager;
        private readonly IEventBusSubscription Subscription;
        private readonly IPluginFactory PluginFactory;
        private readonly ILogger Logger;
        private readonly Shared.EventHandler EventHandler = new Shared.EventHandler();

        public Engine(ILogger logger, IEventFactory eventFactory, IEventBus eventBus, IPluginFactory pluginFactory, IPluginManager pluginManager, ILuaSevice luaService, IApplicationVersionService applicationVersionService) : base("engine")
        {
            EventFactory = eventFactory.Get<IInternalEventFactory>();
            EventBus = eventBus;
            PluginFactory = pluginFactory;
            PluginManager = pluginManager;
            Logger = logger;

            Subscription = EventBus.RegisterListener();

            var internalEventHandler = EventHandler.Get<Slipstream.Shared.EventHandlers.Internal>();

            internalEventHandler.OnInternalCommandPluginRegister += (s, e) => OnCommandPluginRegister(e.Event);
            internalEventHandler.OnInternalCommandPluginUnregister += (s, e) => OnCommandPluginUnregister(e.Event);
            internalEventHandler.OnInternalCommandPluginStates += (s, e) => OnCommandPluginStates(e.Event);

            // Plugins..
            {
                var initFilename = $"init-{applicationVersionService.Version}.lua";

                if (!File.Exists(initFilename))
                {
                    Logger.Information("No {initcfg} file found, creating", initFilename);
                    File.WriteAllText(initFilename, @"
-- This file is auto generated upon startup, if it doesnt exist. So if you
-- ever break it, just rename/delete it, and a new working one is created.
-- There is no auto-reloading of this file - it is only evaluated at startup
print ""Initializing""

-- Listens for samples to play or text to speek. Disabling this will mute all
-- sounds. You can add optional argument ""output"" to device which device to use.
-- Make a small lua script with `audio: send_devices(""AudioPlugin"")` and look at
-- the incoming `AudioOutputDevice` events.The device index is the value you
-- want to use for the value for ""output""
register_plugin({plugin_name = ""AudioPlugin""})

-- Delivers IRacing events as they happen
register_plugin({ plugin_name = ""IRacingPlugin""})

--Connects to Twitch(via the values provided in Settings) and provide
--a way to sende and receive twitch messages.Generate a token here: https://twitchapps.com/tmi/
--register_plugin({ plugin_name = ""TwitchPlugin"", twitch_username = "" < username > "", twitch_token = "" < token > "", twitch_channel = "" < channel > ""})

--Only one of these may be active at a time.ReceiverPlugin listens
-- for TCP connections, while Transmitter will send the events it sees
--to the destination. Both are configured as Txrx in Settings.
-- register_plugin({ plugin_name = ""TransmitterPlugin"", ip = "" < yourip > "", port = < yourport >})
--register_plugin({ plugin_name = ""ReceiverPlugin"", ip = "" < yourip > "", port = < yourport >})

--LuaManagerPlugin listens for FileMonitorPlugin events and acts on them.
-- It will only act on files ending with.lua, which it launches
-- a LuaPlugin for. If the file is modified, it will take down the plugin and
-- launch a new one with the same file.If files are moved out of the directory
-- it is consider as if it were deleted. Deleted files are taken down.
register_plugin({ plugin_name = ""LuaManagerPlugin""})

--FileMonitorPlugin monitors the script directory and sends out events
-- every time a file is created, renamed, modified or deleted
register_plugin({ plugin_name = ""FileMonitorPlugin""})

--Provides save / replay of events. Please be careful if you use this.There is
 --not much filtering, so RegisterPlugin / Unregister plugins will actually make
--slipstream perform these actions
register_plugin({ plugin_name = ""PlaybackPlugin""})
");
                }

                Logger.Information("Loading {initcfg}", initFilename);
                luaService.Parse(filename: initFilename, logPrefix: "INIT");
            }

            // Tell Plugins that we're live - this will make eventbus distribute events
            EventBus.Enabled = true;
        }

        private void OnCommandPluginStates(InternalCommandPluginStates _)
        {
            PluginManager.ForAllPluginsExecute(
                (a) => EventBus.PublishEvent(
                    EventFactory.CreateInternalPluginState(a.Id, a.Name, a.DisplayName, IInternalEventFactory.PluginStatusEnum.Registered)
            ));
        }

        private void OnCommandPluginUnregister(InternalCommandPluginUnregister ev)
        {
            PluginManager.UnregisterPlugin(ev.Id);
        }

        public void UnregisterSubscription(IEventBusSubscription subscription)
        {
            EventBus.UnregisterSubscription(subscription);
        }

        private void OnCommandPluginRegister(Shared.Events.Internal.InternalCommandPluginRegister ev)
        {
            JObject a = JObject.Parse(ev.Configuration);
            Parameters configuration = Parameters.From(a);

            try
            {
                PluginManager.RegisterPlugin(PluginFactory.CreatePlugin(ev.Id, ev.PluginName, configuration));
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Failed creating plugin '{ev.Id}' ('{ev.PluginName}'): {e.Message}");
            }
        }

        protected override void Main()
        {
            while (!Stopped)
            {
                EventHandler.HandleEvent(Subscription.NextEvent(10));
            }
        }

        public new void Dispose()
        {
            PluginManager.Dispose();
            base.Dispose();
        }
    }
}