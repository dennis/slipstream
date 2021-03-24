using Slipstream.Components.AppilcationUpdate.Plugins;

namespace Slipstream.Components.AppilcationUpdate
{
    public class ApplicationUpdate : IComponent
    {
        private const string NAME = nameof(ApplicationUpdatePlugin);
        void IComponent.Register(IComponentRegistrationContext ctx)
        {
            var eventFactory = new EventFactory.ApplicationUpdateEventFactory();

            ctx.RegisterEventFactory(typeof(IApplicationUpdateEventFactory), eventFactory);
            ctx.RegisterEventHandler(typeof(EventHandler.ApplicationUpdateEventHandler));
            ctx.RegisterPlugin(NAME, CreatePlugin);
        }

        private IPlugin CreatePlugin(IComponentPluginCreationContext ctx)
        {
            return new ApplicationUpdatePlugin(
                ctx.EventHandlerController,
                ctx.PluginId,
                ctx.EventFactory.Get<IApplicationUpdateEventFactory>(),
                ctx.Logger.ForContext(typeof(ApplicationUpdate)),
                ctx.EventBus,
                ctx.PluginParameters
            );
        }
    }
}
