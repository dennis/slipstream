using Slipstream.Backend;
using Slipstream.Components.FileMonitor.Plugins;

namespace Slipstream.Components.FileMonitor
{
    internal class FileMonitor : IComponent
    {
        private const string NAME = "FileMonitorPlugin";

        public void Register(IComponentRegistrationContext ctx)
        {
            var eventFactory = new EventFactory.FileMonitorEventFactory();

            ctx.RegisterPlugin(NAME, CreatePlugin);
            ctx.RegisterEventFactory(typeof(IFileMonitorEventFactory), eventFactory);
        }

        private IPlugin CreatePlugin(IComponentPluginCreationContext ctx)
        {
            return new FileMonitorPlugin(
                ctx.EventHandlerController,
                ctx.PluginId,
                ctx.EventFactory.Get<IFileMonitorEventFactory>(),
                ctx.EventBus,
                ctx.PluginParameters
            );
        }
    }
}