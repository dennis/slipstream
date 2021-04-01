using Slipstream.Components.Internal.Services;

namespace Slipstream.Components.Internal
{
    internal class Internal : IComponent
    {
        public void Register(IComponentRegistrationContext ctx)
        {
            var eventFactory = new EventFactory.InternalEventFactory();

            ctx.RegisterEventFactory(typeof(IInternalEventFactory), eventFactory);
            ctx.RegisterLuaGlue(new LuaGlues.CoreLuaGlueFactory(new EventSerdeService()));
            ctx.RegisterLuaGlue(new LuaGlues.HttpLuaGlueFactory(ctx.Logger));
            ctx.RegisterLuaGlue(new LuaGlues.InternalLuaGlueFactory(ctx.EventBus, eventFactory));
            ctx.RegisterLuaGlue(new LuaGlues.StateLuaGlueFactory(new StateService(ctx.Logger, "state.txt")));
        }
    }
}