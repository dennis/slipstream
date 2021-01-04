using Slipstream.Backend.Services;
using Slipstream.Shared;
using Slipstream.Shared.Events.Internal;
using System;
using System.IO;
using static Slipstream.Shared.IEventFactory;

#nullable enable

namespace Slipstream.Backend
{
    partial class Engine : Worker, IEngine, IDisposable
    {
        private readonly IEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private readonly IPluginManager PluginManager;
        private readonly IEventBusSubscription Subscription;
        private readonly IPluginFactory PluginFactory;
        private readonly Shared.EventHandler EventHandler = new Shared.EventHandler();

        public Engine(IEventFactory eventFactory, IEventBus eventBus, IPluginFactory pluginFactory, IPluginManager pluginManager, ILuaSevice luaService, IApplicationVersionService applicationVersionService) : base("engine")
        {
            EventFactory = eventFactory;
            EventBus = eventBus;
            PluginFactory = pluginFactory;
            PluginManager = pluginManager;

            Subscription = EventBus.RegisterListener();

            EventHandler.OnInternalCommandPluginRegister += (s, e) => OnCommandPluginRegister(e.Event);
            EventHandler.OnInternalCommandPluginUnregister += (s, e) => OnCommandPluginUnregister(e.Event);
            EventHandler.OnInternalCommandPluginEnable += (s, e) => PluginManager.FindPluginAndExecute(e.Event.Id, (plugin) => PluginManager.EnablePlugin(plugin));
            EventHandler.OnInternalCommandPluginDisable += (s, e) => PluginManager.FindPluginAndExecute(e.Event.Id, (plugin) => PluginManager.DisablePlugin(plugin));
            EventHandler.OnInternalCommandPluginStates += (s, e) => OnCommandPluginStates(e.Event);
            EventHandler.OnInternalReconfigured += (s, e) => OnInternalReconfigured();

            // Plugins..
            {
                var initFilename = $"init-{applicationVersionService.Version}.lua";

                if (!File.Exists(initFilename))
                {
                    File.WriteAllText(initFilename, @"
-- This file is auto generated upon startup, if it doesnt exist. So if you
-- ever break it, just rename/delete it, and a new working one is created.
-- There is no auto-reloading of this file - it is only evaluated at startup
print ""Initializing"" 

-- Listens for samples to play or text to speek. Disabling this will mute all
-- sounds
register_plugin(""AudioPlugin"", ""AudioPlugin"")
enable_plugin(""AudioPlugin"")

-- Delivers IRacing events as they happen
register_plugin(""IRacingPlugin"", ""IRacingPlugin"")
enable_plugin(""IRacingPlugin"")

-- Connects to Twitch (via the values provided in Settings) and provide
-- a way to sende and receive twitch messages
register_plugin(""TwitchPlugin"", ""TwitchPlugin"")
enable_plugin(""TwitchPlugin"")

-- Only one of these may be active at a time. ReceiverPlugin listens
-- for TCP connections, while Transmitter will send the events it sees
-- to the destination. Both are configured as Txrx in Settings.
register_plugin(""TransmitterPlugin"", ""TransmitterPlugin"")
register_plugin(""ReceiverPlugin"", ""ReceiverPlugin"")

-- FileTriggerPlugin listens for FileMonitorPlugin events and acts on them.
-- Currently it will only act on files ending with .lua, which it launches
-- a plugin for. If the file is modified, it will take down the plugin and
-- launch a new one with the same file. If files are moved out of the directory
-- it is consider as if it were deleted. Deleted files are taken down.
register_plugin(""FileTriggerPlugin"", ""FileTriggerPlugin"")
enable_plugin(""FileTriggerPlugin"")

-- FileMonitorPlugin monitors the script directory and sends out events
-- every time a file is created, renamed, modified or deleted
register_plugin(""FileMonitorPlugin"", ""FileMonitorPlugin"")
enable_plugin(""FileMonitorPlugin"")
");
                }

                luaService.Parse(filename: initFilename, logPrefix: "INIT");
            }

            // Tell Plugins that we're live - this will make eventbus distribute events
            EventBus.Enabled = true;
        }

        private void OnInternalReconfigured()
        {
            PluginManager.RestartReconfigurablePlugins();
        }

        private void OnCommandPluginStates(InternalCommandPluginStates _)
        {
            PluginManager.ForAllPluginsExecute(
                (a) => EventBus.PublishEvent(
                    EventFactory.CreateInternalPluginState(a.Id, a.Name, a.DisplayName, a.Enabled ? PluginStatusEnum.Enabled : PluginStatusEnum.Disabled)
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
            PluginManager.RegisterPlugin(PluginFactory.CreatePlugin(ev.Id, ev.PluginName));
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