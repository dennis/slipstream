using NLua;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;

#nullable enable

namespace Slipstream.Components.Discord
{
    public class LuaGlue : ILuaGlue
    {
        private static DictionaryValidator SendChannelMessageValidator { get; }

        private readonly IEventBus EventBus;
        private readonly IDiscordEventFactory EventFactory;

        static LuaGlue()
        {
            SendChannelMessageValidator = new DictionaryValidator()
                .RequireString("message")
                .RequireLong("channel_id")
                .PermitBool("tts");
        }

        public LuaGlue(IEventBus eventBus, IDiscordEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void SetupLua(NLua.Lua lua)
        {
            lua["discord"] = this;
        }

        public void Loop()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void send_channel_message(LuaTable @params)
        {
            var config = Parameters.From(@params);
            SendChannelMessageValidator.Validate(config);

            var channelId = config.Extract<long>("channel_id");
            var message = config.Extract<string>("message");
            var tts = config.ExtractOrDefault("tts", false);

            EventBus.PublishEvent(EventFactory.CreateDiscordCommandSendMessage((ulong)channelId, message, tts));
        }
    }
}