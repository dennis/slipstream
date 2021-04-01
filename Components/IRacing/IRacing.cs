using Slipstream.Backend;
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
            ctx.RegisterLuaGlue(new LuaGlueFactory(ctx.EventBus, eventFactory));
        }

        private IPlugin CreatePlugin(IComponentPluginCreationContext ctx)
        {
            return new IRacingPlugin(
                ctx.EventHandlerController,
                ctx.PluginId,
                ctx.EventFactory.Get<IIRacingEventFactory>(),
                ctx.EventBus,
                ctx.PluginParameters
            );
        }
    }
}