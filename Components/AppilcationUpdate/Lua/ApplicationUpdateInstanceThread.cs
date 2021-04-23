#nullable enable

using Slipstream.Shared;
using Slipstream.Shared.Lua;
using Squirrel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Slipstream.Components.AppilcationUpdate.Lua
{
    public class ApplicationUpdateInstanceThread : BaseInstanceThread, IApplicationUpdateInstanceThread
    {
        private readonly IEventHandlerController EventHandlerController;
        private readonly IApplicationUpdateEventFactory ApplicationUpdateEventFactory;
        private readonly IEventBus EventBus;
        private readonly IEventBusSubscription Subscription;
        private readonly string UpdateLocation;
        private readonly bool Prerelease;

        public ApplicationUpdateInstanceThread(
            string instanceId,
            string location,
            bool prerelease,
            Serilog.ILogger logger,
            IEventHandlerController eventHandlerController,
            IApplicationUpdateEventFactory applicationUpdateEventFactory,
            IEventBus eventBus,
            IEventBusSubscription subscription) : base(instanceId, logger)
        {
            UpdateLocation = location;
            Prerelease = prerelease;
            EventHandlerController = eventHandlerController;
            ApplicationUpdateEventFactory = applicationUpdateEventFactory;
            EventBus = eventBus;
            Subscription = subscription;
        }

        protected override void Main()
        {
            if (Debugger.IsAttached)
            {
                Logger.Information("Auto update is disabled when a Debugger is attached");
                return;
            }

            Logger.Information($"Auto update, updating from {UpdateLocation}, prerelease: {Prerelease} {Thread.CurrentThread.ManagedThreadId}");

            Init();

            var internalEventHandler = EventHandlerController.Get<Internal.EventHandler.Internal>();
            internalEventHandler.OnInternalShutdown += (_, _e) => Stopping = true;

            while (!Stopping)
            {
                IEvent? @event = Subscription.NextEvent(100);

                if (@event != null)
                {
                    EventHandlerController.HandleEvent(@event);
                }
            }
        }

        private void Init()
        {
            if (string.IsNullOrEmpty(UpdateLocation))
            {
                Logger.Information("Auto update is disabled, no update location specified");
                return;
            }

            using var updateManager = CreateUpdateManager();

            if (updateManager?.IsInstalledApp == true)
            {
                Logger.Information($"Installed application: auto update enabled {Thread.CurrentThread.ManagedThreadId}");

                var applicationUpdate = EventHandlerController.Get<EventHandler.ApplicationUpdateEventHandler>();

                applicationUpdate.OnApplicationUpdateCommandCheckLatestVersion += async (s, e) => await CheckForAppUpdates();
                applicationUpdate.OnApplicationUpdateLatestVersionChanged += async (s, e) => await OnApplicationVersionChanged();

                // Send update event to check for update at startup
                EventBus.PublishEvent(ApplicationUpdateEventFactory.CreateApplicationUpdateCommandCheckLatestVersion());
            }
        }

        private UpdateManager? CreateUpdateManager()
        {
            if (string.IsNullOrEmpty(UpdateLocation))
            {
                return null;
            }

            var isGitHub = UpdateLocation.StartsWith("https://github.com");

            if (isGitHub)
            {
                var asyncUpdateManager = UpdateManager.GitHubUpdateManager(UpdateLocation, prerelease: Prerelease);
                asyncUpdateManager.Wait();
                return asyncUpdateManager.Result;
            }

            return new UpdateManager(UpdateLocation);
        }

        private async Task CheckForAppUpdates()
        {
            Logger.Information("Auto update, checking lastest version");
            using var updateManager = CreateUpdateManager();

            if (updateManager == null)
                return;

            var canUpdate = await updateManager.CheckForUpdate();

            if (canUpdate.ReleasesToApply.Any())
            {
                Logger.Information("Auto update, new version available, raising the event");
                EventBus.PublishEvent(ApplicationUpdateEventFactory.CreateApplicationUpdateLatestVersionChanged(canUpdate.FutureReleaseEntry.Version.ToString()));
            }
            else
            {
                Logger.Information($"Auto update, no update available {Thread.CurrentThread.ManagedThreadId}");
            }
        }

        private async Task OnApplicationVersionChanged()
        {
            using var updateManager = CreateUpdateManager();
            if (updateManager == null)
                return;
            await DoAppUpdates(updateManager);
        }

        private async Task DoAppUpdates(UpdateManager updateManager)
        {
            Logger.Information("Auto updating to the latest version");
            var releaseInfo = await updateManager.UpdateApp();
            Logger.Information($"Auto update, update completed for {releaseInfo.Version}");
        }
    }
}