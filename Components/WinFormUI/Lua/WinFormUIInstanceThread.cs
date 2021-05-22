using Slipstream.Components.Internal;
using Slipstream.Components.Playback;
using Slipstream.Components.UI;
using Slipstream.Components.WinFormUI.Forms;
using Slipstream.Shared;
using Slipstream.Shared.Lua;
using System.Windows.Forms;

namespace Slipstream.Components.WinFormUI.Lua
{
    public class WinFormUIInstanceThread : BaseInstanceThread, IWinFormUIInstanceThread
    {
        private readonly IApplicationVersionService ApplicationVersionService;
        private readonly IEventBus EventBus;
        private readonly IEventHandlerController EventHandlerController;
        private readonly IInternalEventFactory InternalEventFactory;
        private readonly IPlaybackEventFactory PlaybackEventFactory;
        private readonly IUIEventFactory UIEventFactory;

        public WinFormUIInstanceThread(
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
                this,
                InternalEventFactory,
                UIEventFactory,
                PlaybackEventFactory,
                EventBus,
                ApplicationVersionService,
                EventHandlerController));
        }

        // Expose Protected variable for MainWindow to see and react on
        internal bool IsStopping()
        {
            return Stopping;
        }
    }
}