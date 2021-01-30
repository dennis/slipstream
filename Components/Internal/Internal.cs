using Slipstream.Backend.Services;

namespace Slipstream.Components.Internal
{
    internal class Internal : IComponent
    {
        public void Register(IComponentRegistrationContext ctx)
        {
            var eventFactory = new EventFactory.InternalEventFactory();

            ctx.RegisterEventFactory(typeof(IInternalEventFactory), eventFactory);
            ctx.RegisterEventHandler(typeof(EventHandler.Internal));
            ctx.RegisterLuaGlue(new LuaGlues.CoreLuaGlueFactory(new EventSerdeService()));
        }
    }
}