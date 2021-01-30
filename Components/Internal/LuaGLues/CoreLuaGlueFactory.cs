using Slipstream.Backend.Services;

namespace Slipstream.Components.Internal.LuaGlues
{
    internal class CoreLuaGlueFactory : ILuaGlueFactory
    {
        private readonly IEventSerdeService EventSerdeService;

        public CoreLuaGlueFactory(IEventSerdeService eventSerdeService)
        {
            EventSerdeService = eventSerdeService;
        }

        public ILuaGlue CreateLuaGlue()
        {
            return new LuaGlues.CoreLuaGlue(EventSerdeService);
        }
    }
}