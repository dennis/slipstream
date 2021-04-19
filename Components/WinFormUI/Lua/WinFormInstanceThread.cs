using Slipstream.Components.Internal;
using Slipstream.Components.Playback;
using Slipstream.Components.UI;
using Slipstream.Components.WinFormUI.Forms;
using Slipstream.Shared;
using System.Windows.Forms;

namespace Slipstream.Components.WinFormUI.Lua
{
    public class WinFormInstanceThread : BaseInstanceThread, IWinFormInstanceThread
    {
        private readonly IApplicationVersionService ApplicationVersionService;
        private readonly IEventBus EventBus;
        private readonly IEventHandlerController EventHandlerController;
        private readonly IInternalEventFactory InternalEventFactory;
        private readonly IPlaybackEventFactory PlaybackEventFactory;
        private readonly IUIEventFactory UIEventFactory;

        public WinFormInstanceThread(
            string instanceId,
            Serilog.ILogger logger,
            IInternalEventFactory eventFactory,
            IEventBus eventBus,
            IApplicationVersionService applicationVersionService,
            IEventHandlerController eventHandlerController,
            IUIEventFactory uiEventFactory,
            IPlaybackEventFactory playbackEventFactory) : base(instanceId, logger)
        {
            InternalEventFactory = eventFactory;
            EventBus = eventBus;
            ApplicationVersionService = applicationVersionService;
            EventHandlerController = eventHandlerController;
            UIEventFactory = uiEventFactory;
            PlaybackEventFactory = playbackEventFactory;
        }

        protected override void Main()
        {
            Application.Run(new MainWindow(
                InstanceId,
                InternalEventFactory,
                UIEventFactory,
                PlaybackEventFactory,
                EventBus,
                ApplicationVersionService,
                EventHandlerController));
        }
    }
}