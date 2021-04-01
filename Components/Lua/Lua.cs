using Slipstream.Components.Internal.Services;
using Slipstream.Components.Lua.Plugins;
using System.Collections.Generic;

namespace Slipstream.Components.Lua
{
    internal class Lua : IComponent
    {
        public void Register(IComponentRegistrationContext ctx)
        {
            ctx.RegisterPlugin("LuaPlugin", CreateLuaPlugin);
            ctx.RegisterPlugin("LuaManagerPlugin", CreateLuaManagerPlugin);
        }

        private IPlugin CreateLuaManagerPlugin(IComponentPluginCreationContext ctx)
        {
            return new LuaManagerPlugin(
                ctx.EventHandlerController,
                ctx.PluginId,
                ctx.Logger.ForContext(typeof(LuaManagerPlugin)),
                ctx.FileMonitorEventFactory,
                ctx.EventBus,
                ctx.PluginManager,
                ctx.PluginFactory,
                ctx.EventSerdeService);
        }

        private IPlugin CreateLuaPlugin(IComponentPluginCreationContext ctx)
        {
            var luaService = new LuaService(
                ctx.LuaGlues
            );

            return new LuaPlugin(
                ctx.EventHandlerController,
                ctx.PluginId,
                ctx.Logger.ForContext(typeof(LuaPlugin)),
                ctx.LuaEventFactory,
                ctx.InternalEventFactory,
                ctx.EventBus,
                luaService,
                ctx.PluginParameters
            );
        }
    }
}