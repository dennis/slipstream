using NLua;
using Serilog;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;

namespace Slipstream.Components
{
    internal class ComponentPluginCreationContext : IComponentPluginCreationContext
    {
        private readonly ComponentRegistrator ComponentRegistration;

        public IEventHandlerController EventHandlerController
        {
            get { return ComponentRegistration.EventHandlerControllerBuilder.CreateEventHandlerController(); }
        }

        public ILogger Logger
        {
            get { return ComponentRegistration.Logger; }
        }

        public IEventBus EventBus
        {
            get { return ComponentRegistration.EventBus; }
        }

        public IEventFactory EventFactory
        {
            get { return ComponentRegistration.EventFactory; }
        }

        public IServiceLocator ServiceLocator
        {
            get { return ComponentRegistration.ServiceLocator; }
        }

        public string PluginId { get; }

        public string PluginName { get; }

        public Parameters PluginParameters { get; }

        public Lua Lua { get; }

        public ComponentPluginCreationContext(ComponentRegistrator componentRegistration, string pluginId, string pluginName, Parameters pluginParameters)
        {
            ComponentRegistration = componentRegistration;
            PluginId = pluginId;
            PluginName = pluginName;
            PluginParameters = pluginParameters;
            Lua = new Lua();
        }
    }
}