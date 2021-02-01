using Serilog;
using Slipstream.Shared;
using System.IO;

namespace Slipstream.Components.UI
{
    internal class LuaGlueFactory : ILuaGlueFactory
    {
        private readonly ILogger Logger;
        private readonly IEventBus EventBus;
        private readonly IUIEventFactory EventFactory;

        public LuaGlueFactory(ILogger logger, IEventBus eventBus, IUIEventFactory eventFactory)
        {
            Logger = logger;
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public ILuaGlue CreateLuaGlue(IComponentPluginCreationContext ctx)
        {
            string prefix = "";

            if (ctx.PluginName == "LuaPlugin")
            {
                prefix = Path.GetFileName(ctx.PluginParameters.Get<string>("filepath"));
            }
            return new LuaGlue(Logger, EventBus, EventFactory, prefix);
        }
    }
}