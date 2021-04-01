using Slipstream.Backend;
using Slipstream.Components.FileMonitor;
using Slipstream.Components.Internal.Services;
using Slipstream.Components.Lua.Plugins;
using System.Collections.Generic;

namespace Slipstream.Components.Lua
{
    internal class Lua : IComponent
    {
        public void Register(IComponentRegistrationContext ctx)
        {
            var eventFactory = new EventFactory.LuaEventFactory(new EventSerdeService());

            ctx.RegisterPlugin("LuaPlugin", CreateLuaPlugin);
            ctx.RegisterPlugin("LuaManagerPlugin", CreateLuaManagerPlugin);
            ctx.RegisterEventFactory(typeof(ILuaEventFactory), eventFactory);
            ctx.RegisterEventHandler(typeof(EventHandler.Lua));
        }

        private IPlugin CreateLuaManagerPlugin(IComponentPluginCreationContext ctx)
        {
            return new LuaManagerPlugin(
                ctx.EventHandlerController,
                ctx.PluginId,
                ctx.Logger.ForContext(typeof(LuaManagerPlugin)),
                ctx.EventFactory.Get<IFileMonitorEventFactory>(),
                ctx.EventBus,
                ctx.PluginManager,
                ctx.PluginFactory,
                ctx.EventSerdeService);
        }

        private IPlugin CreateLuaPlugin(IComponentPluginCreationContext ctx)
        {
            List<ILuaGlue> states = new List<ILuaGlue>();

            foreach (var factories in ctx.LuaGlueFactories)
            {
                states.Add(factories.CreateLuaGlue(ctx));
            }

            var luaService = new LuaService(
                states
            );

            return new LuaPlugin(
                ctx.EventHandlerController,
                ctx.PluginId,
                ctx.Logger.ForContext(typeof(LuaPlugin)),
                ctx.EventFactory,
                ctx.EventBus,
                luaService,
                ctx.PluginParameters
            );
        }
    }
}