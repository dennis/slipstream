using Slipstream.Backend;
using Slipstream.Backend.Services;
using Slipstream.Components.FileMonitor;
using Slipstream.Components.Lua.Plugins;

namespace Slipstream.Components.Lua
{
    internal class Lua : IComponent
    {
        public void Register(IComponentRegistrationContext ctx)
        {
            var eventFactory = new EventFactory.LuaEventFactory(new EventSerdeService()); // FIXME

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
                ctx.ServiceLocator);
        }

        private IPlugin CreateLuaPlugin(IComponentPluginCreationContext ctx)
        {
            var luaService = new LuaService(
                ctx.Logger.ForContext(typeof(LuaPlugin)),
                ctx.EventFactory,
                ctx.EventBus,
                ctx.ServiceLocator.Get<IStateService>(),
                ctx.ServiceLocator.Get<IEventSerdeService>(),
                ctx.LuaGlues
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