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
        }

        private IPlugin CreatePlugin(IComponentPluginCreationContext ctx)
        {
            return new IRacingPlugin(
                ctx.EventHandlerController,
                ctx.PluginId,
                ctx.IRacingEventFactory,
                ctx.EventBus,
                ctx.PluginParameters
            );
        }
    }
}