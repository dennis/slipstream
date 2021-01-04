using NLua;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Backend.Services.LuaServiceLib
{
    public partial class LuaContext
    {
        public class TwitchMethodCollection
        {
            private readonly IEventBus EventBus;
            private readonly IEventFactory EventFactory;

            public static TwitchMethodCollection Register(IEventBus eventBus, IEventFactory eventFactory, Lua lua)
            {
                var m = new TwitchMethodCollection(eventBus, eventFactory);
                m.Register(lua);
                return m;
            }

            public TwitchMethodCollection(IEventBus eventBus, IEventFactory eventFactory)
            {
                EventBus = eventBus;
                EventFactory = eventFactory;
            }

            public void Register(Lua lua)
            {
                lua["twitch"] = this;
                lua.DoString(@"
function send_twitch_message(a); twitch:send_channel_message(a); end
");
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
            public void send_channel_message(string message)
            {
                EventBus.PublishEvent(EventFactory.CreateTwitchCommandSendMessage(message));
            }
        }
    }
}
