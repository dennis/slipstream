using Slipstream.Components.AppilcationUpdate.Plugins;

namespace Slipstream.Components.AppilcationUpdate
{
    public class ApplicationUpdate : IComponent
    {
        private const string NAME = nameof(ApplicationUpdatePlugin);

        void IComponent.Register(IComponentRegistrationContext ctx)
        {
            ctx.RegisterPlugin(NAME, CreatePlugin);
        }

        private IPlugin CreatePlugin(IComponentPluginCreationContext ctx)
        {
            return new ApplicationUpdatePlugin(
                ctx.EventHandlerController,
                ctx.PluginId,
                ctx.ApplicationUpdateEventFactory,
                ctx.Logger.ForContext(typeof(ApplicationUpdate)),
                ctx.EventBus,
                ctx.PluginParameters
            );
        }
    }
}