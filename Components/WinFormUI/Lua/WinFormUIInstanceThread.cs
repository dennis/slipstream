using Slipstream.Components.Internal;
using Slipstream.Components.Playback;
using Slipstream.Components.WinFormUI.Forms;
using Slipstream.Shared;
using Slipstream.Shared.Lua;

using System;
using System.Windows.Forms;

namespace Slipstream.Components.WinFormUI.Lua
{
    public class WinFormUIInstanceThread : BaseInstanceThread, IWinFormUIInstanceThread
    {
        private readonly IApplicationVersionService ApplicationVersionService;
        private readonly IEventHandlerController EventHandlerController;
        private readonly IPlaybackEventFactory PlaybackEventFactory;
        private readonly IEventSerdeService EventSerdeService;
        private readonly IWinFormUIEventFactory UIEventFactory;

        public WinFormUIInstanceThread(
            string luaLibraryName,
            string instanceId,
            Serilog.ILogger logger,
            IInternalEventFactory eventFactory,
            IEventBus eventBus,
            IApplicationVersionService applicationVersionService,
            IEventHandlerController eventHandlerController,
            IWinFormUIEventFactory uiEventFactory,
            IPlaybackEventFactory playbackEventFactory,
            IEventSerdeService eventSerdeService
        ) : base(luaLibraryName, instanceId, logger, eventHandlerController, eventBus, eventFactory)
        {
            InternalEventFactory = eventFactory;
            EventBus = eventBus;
            ApplicationVersionService = applicationVersionService;
            EventHandlerController = eventHandlerController;
            UIEventFactory = uiEventFactory;
            PlaybackEventFactory = playbackEventFactory;
            EventSerdeService = eventSerdeService;
        }

        [STAThreadAttribute]
        protected override void Main()
        {
            Application.Run(new MainWindow(
                InstanceId,
                this,
                InternalEventFactory,
                UIEventFactory,
                PlaybackEventFactory,
                EventBus,
                ApplicationVersionService,
                EventHandlerController,
                EventSerdeService
           ));
        }

        // Expose Protected variable for MainWindow to see and react on
        internal bool IsStopping()
        {
            return Stopping;
        }
    }
}