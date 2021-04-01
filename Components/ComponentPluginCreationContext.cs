using Serilog;
using Slipstream.Backend;
using Slipstream.Components.AppilcationUpdate;
using Slipstream.Components.Audio;
using Slipstream.Components.Discord;
using Slipstream.Components.FileMonitor;
using Slipstream.Components.Internal;
using Slipstream.Components.IRacing;
using Slipstream.Components.Lua;
using Slipstream.Components.Playback;
using Slipstream.Components.Twitch;
using Slipstream.Components.UI;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using System.Collections.Generic;

namespace Slipstream.Components
{
    internal class ComponentPluginCreationContext : IComponentPluginCreationContext
    {
        private readonly ComponentRegistrator ComponentRegistration;

        public IEventHandlerController EventHandlerController { get; }

        public ILogger Logger
        {
            get { return ComponentRegistration.Logger; }
        }

        public IEventBus EventBus
        {
            get { return ComponentRegistration.EventBus; }
        }

        public string PluginId { get; }

        public string PluginName { get; }

        public Parameters PluginParameters { get; }

        public NLua.Lua Lua { get; }

        public IPluginManager PluginManager { get; }

        public IPluginFactory PluginFactory { get; }

        public IEventSerdeService EventSerdeService { get; }

        public IInternalEventFactory InternalEventFactory { get; }

        public IUIEventFactory UIEventFactory { get; }

        public IPlaybackEventFactory PlaybackEventFactory { get; }

        public ILuaEventFactory LuaEventFactory { get; }

        public IApplicationUpdateEventFactory ApplicationUpdateEventFactory { get; }

        public IFileMonitorEventFactory FileMonitorEventFactory { get; }

        public IAudioEventFactory AudioEventFactory { get; }

        public IDiscordEventFactory DiscordEventFactory { get; }

        public IIRacingEventFactory IRacingEventFactory { get; }

        public ITwitchEventFactory TwitchEventFactory { get; }

        public IEnumerable<ILuaGlue> LuaGlues { get; }

        public ComponentPluginCreationContext(
                ComponentRegistrator componentRegistration,
                IPluginManager pluginManager,
                IPluginFactory pluginFactory,
                string pluginId,
                string pluginName,
                Parameters pluginParameters,
                IEventSerdeService eventSerdeService,
                IEventHandlerController eventHandlerController,
                IInternalEventFactory internalEventFactory,
                IUIEventFactory uiEventFactory,
                IPlaybackEventFactory playbackEventFactory,
                ILuaEventFactory luaEventFactory,
                IApplicationUpdateEventFactory applicationUpdateEventFactory,
                IFileMonitorEventFactory fileMonitorEventFactory,
                IAudioEventFactory audioEventFactory,
                IDiscordEventFactory discordEventFactory,
                IIRacingEventFactory iRacingEventFactory,
                ITwitchEventFactory twitchEventFactory,
                IEnumerable<ILuaGlue> luaGlues)
        {
            ComponentRegistration = componentRegistration;
            PluginManager = pluginManager;
            PluginFactory = pluginFactory;
            PluginId = pluginId;
            PluginName = pluginName;
            PluginParameters = pluginParameters;
            EventSerdeService = eventSerdeService;
            EventHandlerController = eventHandlerController;
            InternalEventFactory = internalEventFactory;
            UIEventFactory = uiEventFactory;
            PlaybackEventFactory = playbackEventFactory;
            LuaEventFactory = luaEventFactory;
            ApplicationUpdateEventFactory = applicationUpdateEventFactory;
            FileMonitorEventFactory = fileMonitorEventFactory;
            AudioEventFactory = audioEventFactory;
            DiscordEventFactory = discordEventFactory;
            IRacingEventFactory = iRacingEventFactory;
            TwitchEventFactory = twitchEventFactory;
            LuaGlues = luaGlues;
            Lua = new NLua.Lua();
        }
    }
}