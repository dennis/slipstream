﻿using Slipstream.Shared;

namespace Slipstream.Components.Audio
{
    internal class LuaGlue : ILuaGlue
    {
        private readonly IEventBus EventBus;
        private readonly IAudioEventFactory EventFactory;

        public LuaGlue(IEventBus eventBus, IAudioEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void SetupLua(NLua.Lua lua)
        {
            lua["audio"] = this;
            lua.DoString(@"
function say(plugin_id, message, volume); audio:say(plugin_id,message, volume); end
function play(plugin_id, filename, volume); audio:play(plugin_id, filename, volume); end
");
        }

        public void Loop()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void say(string pluginId, string message, float volume)
        {
            EventBus.PublishEvent(EventFactory.CreateAudioCommandSay(pluginId, message, volume));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void play(string pluginId, string filename, float volume)
        {
            EventBus.PublishEvent(EventFactory.CreateAudioCommandPlay(pluginId, filename, volume));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void send_devices(string pluginId)
        {
            EventBus.PublishEvent(EventFactory.CreateAudioCommandSendDevices(pluginId));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void set_output(string pluginId, int deviceIdx)
        {
            EventBus.PublishEvent(EventFactory.CreateAudioCommandSetOutputDevice(pluginId, deviceIdx));
        }
    }
}