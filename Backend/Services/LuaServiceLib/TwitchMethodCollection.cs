﻿using NLua;
using Slipstream.Shared;
using Slipstream.Shared.Factories;

#nullable enable

namespace Slipstream.Backend.Services.LuaServiceLib
{
    public partial class LuaContext
    {
        public class TwitchMethodCollection
        {
            private readonly IEventBus EventBus;
            private readonly ITwitchEventFactory EventFactory;

            public static TwitchMethodCollection Register(IEventBus eventBus, ITwitchEventFactory eventFactory, Lua lua)
            {
                var m = new TwitchMethodCollection(eventBus, eventFactory);
                m.Register(lua);
                return m;
            }

            public TwitchMethodCollection(IEventBus eventBus, ITwitchEventFactory eventFactory)
            {
                EventBus = eventBus;
                EventFactory = eventFactory;
            }

            public void Register(Lua lua)
            {
                lua["twitch"] = this;
                lua.DoString(@"
function send_twitch_message(msg); twitch:send_channel_message(msg); end
function send_twitch_whisper(to, msg); twitch:send_whisper_message(to, msg); end
");
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
            public void send_channel_message(string message)
            {
                EventBus.PublishEvent(EventFactory.CreateTwitchCommandSendMessage(message));
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
            public void send_whisper_message(string to, string message)
            {
                EventBus.PublishEvent(EventFactory.CreateTwitchCommandSendWhisper(to, message));
            }
        }
    }
}
