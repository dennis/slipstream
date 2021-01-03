using NLua;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Backend.Services.LuaServiceLib
{
    public partial class LuaContext
    {
        public class AudioMethodCollection
        {
            private readonly IEventBus EventBus;
            private readonly IEventFactory EventFactory;

            public static AudioMethodCollection Register(IEventBus eventBus, IEventFactory eventFactory, Lua lua)
            {
                var m = new AudioMethodCollection(eventBus, eventFactory);
                m.Register(lua);
                return m;
            }

            public AudioMethodCollection(IEventBus eventBus, IEventFactory eventFactory)
            {
                EventBus = eventBus;
                EventFactory = eventFactory;
            }

            public void Register(Lua lua)
            {
                lua["audio"] = this;
                lua.DoString(@"
function say(a,b); audio:say(a,b); end
function play(a,b); audio:play(a,b); end
");
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
            public void say(string message, float volume)
            {
                EventBus.PublishEvent(EventFactory.CreateAudioCommandSay(message, volume));
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
            public void play(string filename, float volume)
            {
                EventBus.PublishEvent(EventFactory.CreateAudioCommandPlay(filename, volume));
            }
        }
    }
}
