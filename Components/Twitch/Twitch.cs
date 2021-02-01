using Slipstream.Backend;
using Slipstream.Components.Twitch.Plugins;

namespace Slipstream.Components.Twitch
{
    internal class Twitch : IComponent
    {
        public void Register(IComponentRegistrationContext ctx)
        {
            var eventFactory = new EventFactory.TwitchEventFactory();

            ctx.RegisterPlugin("TwitchPlugin", CreatePlugin);
            ctx.RegisterEventFactory(typeof(ITwitchEventFactory), eventFactory);
            ctx.RegisterEventHandler(typeof(EventHandler.Twitch));
            ctx.RegisterLuaGlue(new LuaGlueFactory(ctx.EventBus, eventFactory));
        }

        private IPlugin CreatePlugin(IComponentPluginCreationContext ctx)
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