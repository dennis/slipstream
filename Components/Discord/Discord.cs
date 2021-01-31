using Slipstream.Backend;

namespace Slipstream.Components.Discord
{
    internal class Discord : IComponent
    {
        public void Register(IComponentRegistrationContext ctx)
        {
            var eventFactory = new EventFactory.DiscordEventFactory();

            ctx.RegisterPlugin("DiscordPlugin", CreatePlugin);
            ctx.RegisterEventHandler(typeof(EventHandler.DiscordEventHandler));
            ctx.RegisterEventFactory(typeof(IDiscordEventFactory), eventFactory);
            ctx.RegisterLuaGlue(new LuaGlueFactory(ctx.EventBus, eventFactory));
        }

        private IPlugin CreatePlugin(IComponentPluginCreationContext ctx)
        {
            return new Plugins.DiscordPlugin(
                ctx.EventHandlerController,
                ctx.PluginId,
                ctx.EventBus,
                ctx.EventFactory.Get<IDiscordEventFactory>(),
                ctx.PluginParameters
            );
        }
    }
}