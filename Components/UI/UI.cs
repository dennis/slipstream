namespace Slipstream.Components.UI
{
    internal class UI : IComponent
    {
        public void Register(IComponentRegistrationContext ctx)
        {
            var eventFactory = new EventFactory.UIEventFactory();

            ctx.RegisterEventFactory(typeof(IUIEventFactory), eventFactory);

            ctx.RegisterLuaGlue(new LuaGlueFactory(ctx.Logger, ctx.EventBus, eventFactory));
        }
    }
}