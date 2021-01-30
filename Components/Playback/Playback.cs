using Slipstream.Backend;
using Slipstream.Backend.Plugins;
using Slipstream.Components.Playback.Plugins;

namespace Slipstream.Components.Playback
{
    internal class Playback : IComponent
    {
        public void Register(IComponentRegistrationContext ctx)
        {
            var eventFactory = new EventFactory.PlaybackEventFactory();

            ctx.RegisterPlugin("PlaybackPlugin", CreatePlugin);
            ctx.RegisterEventFactory(typeof(IPlaybackEventFactory), eventFactory);
            ctx.RegisterEventHandler(typeof(EventHandler.Playback));
            ctx.RegisterLuaGlue(new LuaGlue(ctx.EventBus, eventFactory));
        }

        private IPlugin CreatePlugin(IComponentPluginCreationContext ctx)
        {
            return new PlaybackPlugin(
                ctx.EventHandlerController,
                ctx.PluginId,
                ctx.Logger,
                ctx.EventBus,
                ctx.ServiceLocator
            );
        }
    }
}