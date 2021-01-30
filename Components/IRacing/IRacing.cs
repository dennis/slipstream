using Slipstream.Backend;
using Slipstream.Backend.Plugins;
using Slipstream.Components.IRacing.Plugins;

namespace Slipstream.Components.IRacing
{
    internal class IRacing : IComponent
    {
        private const string NAME = "IRacingPlugin";

        public void Register(IComponentRegistrationContext ctx)
        {
            var eventFactory = new EventFactory.IRacingEventFactory();

            ctx.RegisterPlugin(NAME, CreatePlugin);
            ctx.RegisterEventFactory(typeof(IIRacingEventFactory), eventFactory);
            ctx.RegisterEventHandler(typeof(EventHandler.IRacing));
            ctx.RegisterLuaGlue(new LuaGlue(ctx.EventBus, eventFactory));
        }

        private IPlugin CreatePlugin(IComponentPluginCreationContext ctx)
        {
            //                "IRacingPlugin" => new IRacingPlugin(EventHandlerControllerBuilder.CreateEventHandlerController(), pluginId, IRacingEventFactory, eventBus),

            return new IRacingPlugin(
                ctx.EventHandlerController,
                ctx.PluginId,
                ctx.EventFactory.Get<IIRacingEventFactory>(),
                ctx.EventBus
            );
        }
    }
}