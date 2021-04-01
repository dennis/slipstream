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
        }

        private IPlugin CreatePlugin(IComponentPluginCreationContext ctx)
        {
            return new TwitchPlugin(
                ctx.EventHandlerController,
                ctx.PluginId,
                ctx.Logger.ForContext(typeof(Twitch)),
                ctx.TwitchEventFactory,
                ctx.EventBus,
                ctx.PluginParameters
            );
        }
    }
}