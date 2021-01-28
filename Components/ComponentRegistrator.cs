using Serilog;
using Slipstream.Backend;
using Slipstream.Shared;
using System;
using System.Collections.Generic;

namespace Slipstream.Components
{
    internal class ComponentRegistrator : IComponentRegistrationContext
    {
        private readonly List<ILuaGlue> LuaGlues;
        private readonly Dictionary<string, Func<IComponentPluginCreationContext, IPlugin>> Plugins;

        public ILogger Logger { get; internal set; }

        public IEventBus EventBus { get; internal set; }

        public IEventFactory EventFactory { get; internal set; }

        public IServiceLocator ServiceLocator { get; internal set; }

        public EventHandlerControllerBuilder EventHandlerControllerBuilder { get; }

        public ComponentRegistrator(
            Dictionary<string, Func<IComponentPluginCreationContext, IPlugin>> plugins,
            List<ILuaGlue> luaGlues,
            IEventFactory eventFactory,
            ILogger logger,
            IEventBus eventBus,
            EventHandlerControllerBuilder eventHandlerControllerBuilder,
            IServiceLocator serviceLocator
        )
        {
            Plugins = plugins;
            LuaGlues = luaGlues;
            EventHandlerControllerBuilder = eventHandlerControllerBuilder;
            EventFactory = eventFactory;
            Logger = logger;
            EventBus = eventBus;
            ServiceLocator = serviceLocator;
        }

        public void RegisterEventFactory<T>(Type type, T factory)
        {
            EventFactory.Add(type, factory);
        }

        public void RegisterEventHandler(Type type)
        {
            EventHandlerControllerBuilder.Add(type);
        }

        public void RegisterPlugin(string name, Func<IComponentPluginCreationContext, IPlugin> plugin)
        {
            Plugins.Add(name, plugin);
        }

        public void RegisterLuaGlue(ILuaGlue luaGlue)
        {
            LuaGlues.Add(luaGlue);
        }
    }
}