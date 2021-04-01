using Slipstream.Components.Internal;
using Slipstream.Components.Playback;
using Slipstream.Components.UI;
using Slipstream.Components.WinFormUI.Forms;
using Slipstream.Shared;
using System.Windows.Forms;

#nullable enable

namespace Slipstream.Components.WinFormUI.Plugins
{
    internal class WinFormUIPlugin : BasePlugin
    {
        private readonly IEventBus EventBus;
        private readonly IApplicationVersionService ApplicationVersionService;
        private readonly IInternalEventFactory InternalEventFactory;
        private readonly IUIEventFactory UIEventFactory;
        private readonly IPlaybackEventFactory PlaybackEventFactory;

        public WinFormUIPlugin(IEventHandlerController eventHandlerController, string id, IInternalEventFactory internalEventFactory, IUIEventFactory uiEventFactory, IPlaybackEventFactory playbackEventFactory, IEventBus eventBus, IApplicationVersionService applicationVersionService) : base(eventHandlerController, id, "WinFormUIPlugin", id, true, true)
        {
            InternalEventFactory = internalEventFactory;
            UIEventFactory = uiEventFactory;
            PlaybackEventFactory = playbackEventFactory;
            EventBus = eventBus;
            ApplicationVersionService = applicationVersionService;
        }

        public override void Run()
        {
            Application.Run(new MainWindow(InternalEventFactory, UIEventFactory, PlaybackEventFactory, EventBus, ApplicationVersionService, EventHandlerController));
        }
    }
}