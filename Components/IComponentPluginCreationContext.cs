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
using Slipstream.Shared.Helpers.StrongParameters;
using System.Collections.Generic;

namespace Slipstream.Components
{
    internal interface IComponentPluginCreationContext : IComponentPluginDependencies
    {
        string PluginId { get; }
        string PluginName { get; }
        Parameters PluginParameters { get; }
        IPluginManager PluginManager { get; }
        IPluginFactory PluginFactory { get; }
        IEventSerdeService EventSerdeService { get; }
        IInternalEventFactory InternalEventFactory { get; }
        IUIEventFactory UIEventFactory { get; }
        IPlaybackEventFactory PlaybackEventFactory { get; }
        ILuaEventFactory LuaEventFactory { get; }
        IApplicationUpdateEventFactory ApplicationUpdateEventFactory { get; }
        IFileMonitorEventFactory FileMonitorEventFactory { get; }
        IAudioEventFactory AudioEventFactory { get; }
        IDiscordEventFactory DiscordEventFactory { get; }
        IIRacingEventFactory IRacingEventFactory { get; }
        ITwitchEventFactory TwitchEventFactory { get; }
        IEnumerable<ILuaGlue> LuaGlues { get; }
    }
}