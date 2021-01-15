using NLua;
using Slipstream.Shared;
using Slipstream.Shared.Factories;

#nullable enable

namespace Slipstream.Backend.Services.LuaServiceLib
{
    public partial class LuaContext
    {
        public class AudioMethodCollection
        {
            private readonly IEventBus EventBus;
            private readonly IAudioEventFactory EventFactory;

            public static AudioMethodCollection Register(IEventBus eventBus, IAudioEventFactory eventFactory, Lua lua)
            {
                var m = new AudioMethodCollection(eventBus, eventFactory);
                m.Register(lua);
                return m;
            }

            public AudioMethodCollection(IEventBus eventBus, IAudioEventFactory eventFactory)
            {
                EventBus = eventBus;
                EventFactory = eventFactory;
            }

            public void Register(Lua lua)
            {
                lua["audio"] = this;
                lua.DoString(@"
function say(plugin_id, message, volume); audio:say(plugin_id,message, volume); end
function play(plugin_id, filename, volume); audio:play(plugin_id, filename, volume); end

");
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
}
