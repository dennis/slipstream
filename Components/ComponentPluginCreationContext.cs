using Serilog;
using Slipstream.Backend;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using System.Collections.Generic;

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

        public NLua.Lua Lua { get; }

        public List<ILuaGlue> LuaGlues { get; }

        public IPluginManager PluginManager { get; }

        public IPluginFactory PluginFactory { get; }

        public ComponentPluginCreationContext(
            ComponentRegistrator componentRegistration,
            IPluginManager pluginManager,
            IPluginFactory pluginFactory,
            List<ILuaGlue> luaGlues,
            string pluginId,
            string pluginName,
            Parameters pluginParameters)
        {
            ComponentRegistration = componentRegistration;
            PluginManager = pluginManager;
            PluginFactory = pluginFactory;
            LuaGlues = luaGlues;
            PluginId = pluginId;
            PluginName = pluginName;
            PluginParameters = pluginParameters;
            Lua = new NLua.Lua();
        }
    }
}