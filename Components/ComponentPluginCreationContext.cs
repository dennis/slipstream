using Serilog;
using Slipstream.Backend;
using Slipstream.Components.Internal;
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

        public string PluginId { get; }

        public string PluginName { get; }

        public Parameters PluginParameters { get; }

        public NLua.Lua Lua { get; }

        public List<ILuaGlueFactory> LuaGlueFactories { get; }

        public IPluginManager PluginManager { get; }

        public IPluginFactory PluginFactory { get; }

        public IEventSerdeService EventSerdeService { get; }

        public ComponentPluginCreationContext(
                ComponentRegistrator componentRegistration,
                IPluginManager pluginManager,
                IPluginFactory pluginFactory,
                List<ILuaGlueFactory> luaGlueFactories,
                string pluginId,
                string pluginName,
                Parameters pluginParameters,
                IEventSerdeService eventSerdeService)
        {
            ComponentRegistration = componentRegistration;
            PluginManager = pluginManager;
            PluginFactory = pluginFactory;
            LuaGlueFactories = luaGlueFactories;
            PluginId = pluginId;
            PluginName = pluginName;
            PluginParameters = pluginParameters;
            EventSerdeService = eventSerdeService;
            Lua = new NLua.Lua();
        }
    }
}