using Slipstream.Backend;

namespace Slipstream.Components.Discord
{
    internal class Discord : IComponent
    {
        public void Register(IComponentRegistrationContext ctx)
        {
            var eventFactory = new EventFactory.DiscordEventFactory();

            ctx.RegisterPlugin("DiscordPlugin", CreatePlugin);
            ctx.RegisterLuaGlue(new LuaGlueFactory(ctx.EventBus, eventFactory));
        }

        private IPlugin CreatePlugin(IComponentPluginCreationContext ctx)
        {
            return new Plugins.DiscordPlugin(
                ctx.EventHandlerController,
                ctx.PluginId,
                ctx.EventBus,
                ctx.DiscordEventFactory,
                ctx.PluginParameters
            );
        }
    }
}