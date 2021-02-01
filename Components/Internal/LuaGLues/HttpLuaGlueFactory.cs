using Serilog;

namespace Slipstream.Components.Internal.LuaGlues
{
    internal class HttpLuaGlueFactory : ILuaGlueFactory
    {
        private readonly ILogger Logger;

        public HttpLuaGlueFactory(ILogger logger)
        {
            Logger = logger;
        }

        public ILuaGlue CreateLuaGlue(IComponentPluginCreationContext ctx)
        {
            return new HttpLuaGlue(Logger);
        }
    }
}