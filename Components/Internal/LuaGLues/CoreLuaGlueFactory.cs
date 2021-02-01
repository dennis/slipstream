namespace Slipstream.Components.Internal.LuaGlues
{
    internal class CoreLuaGlueFactory : ILuaGlueFactory
    {
        private readonly IEventSerdeService EventSerdeService;

        public CoreLuaGlueFactory(IEventSerdeService eventSerdeService)
        {
            EventSerdeService = eventSerdeService;
        }

        public ILuaGlue CreateLuaGlue(IComponentPluginCreationContext ctx)
        {
            return new CoreLuaGlue(EventSerdeService);
        }
    }
}