using Slipstream.Components.FileMonitor.Plugins;

namespace Slipstream.Components.FileMonitor
{
    internal class FileMonitor : IComponent
    {
        private const string NAME = "FileMonitorPlugin";

        public void Register(IComponentRegistrationContext ctx)
        {
            ctx.RegisterPlugin(NAME, CreatePlugin);
        }

        private IPlugin CreatePlugin(IComponentPluginCreationContext ctx)
        {
            return new FileMonitorPlugin(
                ctx.EventHandlerController,
                ctx.PluginId,
                ctx.FileMonitorEventFactory,
                ctx.EventBus,
                ctx.PluginParameters
            );
        }
    }
}