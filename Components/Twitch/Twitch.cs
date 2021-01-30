using Slipstream.Backend;
using Slipstream.Components.Twitch.Plugins;
using Slipstream.Shared;

namespace Slipstream.Components.Twitch
{
    internal class LuaGlueFactory : ILuaGlueFactory
    {
        private readonly IEventBus EventBus;
        private readonly ITwitchEventFactory EventFactory;

        public LuaGlueFactory(IEventBus eventBus, ITwitchEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public ILuaGlue CreateLuaGlue()
        {
            return new LuaGlue(EventBus, EventFactory);
        }
    }

    internal class Twitch : IComponent
    {
        private const string NAME = "TwitchPlugin";

        public void Register(IComponentRegistrationContext ctx)
        {
            var eventFactory = new EventFactory.TwitchEventFactory();

            ctx.RegisterPlugin(NAME, CreateAudioPlugin);
            ctx.RegisterEventFactory(typeof(ITwitchEventFactory), eventFactory);
            ctx.RegisterEventHandler(typeof(EventHandler.Twitch));
            ctx.RegisterLuaGlue(new LuaGlueFactory(ctx.EventBus, eventFactory));
        }

        private IPlugin CreateAudioPlugin(IComponentPluginCreationContext ctx)
        {
            return new TwitchPlugin(
                ctx.EventHandlerController,
                ctx.PluginId,
                ctx.Logger.ForContext(typeof(Twitch)),
                ctx.EventFactory.Get<ITwitchEventFactory>(),
                ctx.EventBus,
                ctx.PluginParameters
            );
        }
    }
}