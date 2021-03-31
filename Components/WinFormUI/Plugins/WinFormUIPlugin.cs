using Slipstream.Components.WinFormUI.Forms;
using Slipstream.Shared;
using System.Windows.Forms;

#nullable enable

namespace Slipstream.Components.WinFormUI.Plugins
{
    internal class WinFormUIPlugin : BasePlugin
    {
        private readonly IEventFactory EventFactory;
        private readonly IEventBus EventBus;
        private readonly IApplicationVersionService ApplicationVersionService;

        public WinFormUIPlugin(IEventHandlerController eventHandlerController, string id, IEventFactory eventFactory, IEventBus eventBus, IApplicationVersionService applicationVersionService) : base(eventHandlerController, id, "WinFormUIPlugin", id, true, true)
        {
            EventFactory = eventFactory;
            EventBus = eventBus;
            ApplicationVersionService = applicationVersionService;
        }

        public override void Run()
        {
            Application.Run(new MainWindow(EventFactory, EventBus, ApplicationVersionService, EventHandlerController));
        }
    }
}