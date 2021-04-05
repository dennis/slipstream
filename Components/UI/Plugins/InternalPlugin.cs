using Serilog;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using System.Collections.Generic;
using System.IO;

namespace Slipstream.Components.UI.Plugins
{
    public class UIPlugin : BasePlugin, IPlugin
    {
        private readonly IUIEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private readonly ILogger Logger;
        private readonly string Prefix;

        public UIPlugin(
            IEventHandlerController eventHandlerController,
            string id,
            ILogger logger,
            IEventBus eventBus,
            IUIEventFactory eventFactory,
            Parameters configuration
        ) : base(eventHandlerController, id, nameof(UIPlugin), id, true)
        {
            EventFactory = eventFactory;
            Logger = logger;
            EventBus = eventBus;
            Prefix = Path.GetFileName(configuration.GetOrDefault("filepath", ""));
        }

        public IEnumerable<ILuaGlue> CreateLuaGlues()
        {
            return new ILuaGlue[]
            {
                new LuaGlue(Logger, EventBus, EventFactory, Prefix)
        };
        }
    }
}