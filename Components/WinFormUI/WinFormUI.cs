using Slipstream.Components.WinFormUI.Plugins;
using Slipstream.Shared;

namespace Slipstream.Components.WinFormUI
{
    internal class WinFormUI : IComponent
    {
        private const string NAME = "WinFormUIPlugin";

        public void Register(IComponentRegistrationContext ctx)
        {
            ctx.RegisterPlugin(NAME, CreatePlugin);
        }

        private IPlugin CreatePlugin(IComponentPluginCreationContext ctx)
        {
            return new WinFormUIPlugin(
                ctx.EventHandlerController,
                ctx.PluginId,
                ctx.EventFactory,
                ctx.EventBus,
                new ApplicationVersionService()
            );
        }
    }
}