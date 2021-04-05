using Serilog;
using Slipstream.Components.Internal.Events;
using Slipstream.Shared;
using Slipstream.Shared.Helpers.StrongParameters;
using Slipstream.Shared.Helpers.StrongParameters.Validators;
using Squirrel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Slipstream.Components.AppilcationUpdate.Plugins
{
    public class ApplicationUpdatePlugin : BasePlugin, IPlugin
    {
        private readonly EventHandler.ApplicationUpdateEventHandler applicationUpdate;
        private readonly IApplicationUpdateEventFactory applicationUpdateEventFactory;
        private readonly IEventBus eventBus;
        private string updateLocation;
        private bool prerelease;
        private bool updateNotified = false;

        public ILogger Logger { get; }
        internal static DictionaryValidator ConfigurationValidator { get; }

        static ApplicationUpdatePlugin()
        {
            ConfigurationValidator = new DictionaryValidator()
                .RequireString("updateLocation")
                .PermitBool("prerelease");
        }

        public ApplicationUpdatePlugin(
            IEventHandlerController eventHandlerController,
            string id,
            IApplicationUpdateEventFactory applicationUpdateEventFactory,
            ILogger logger,
            IEventBus eventBus,
            Parameters configuration)
            : base(eventHandlerController, id, nameof(ApplicationUpdatePlugin), id, true)
        {
            ConfigurationValidator.Validate(configuration);

            this.applicationUpdateEventFactory = applicationUpdateEventFactory;
            Logger = logger;
            this.eventBus = eventBus;

            SubscribeToInternalEvents();

            if (Debugger.IsAttached)
            {
                Logger.Information("Auto update is disabled when a Debugger is attached");
                return;
            }

            // ToDo: this can be injected in the constructor
            applicationUpdate = EventHandlerController.Get<EventHandler.ApplicationUpdateEventHandler>();

            var updateLocation = configuration.ExtractOrDefault("updateLocation", string.Empty);
            var prerelease = configuration.ExtractOrDefault("prerelease", false);

            Logger.Information($"Auto update, updating from {updateLocation}, prerelease: {prerelease}");

            Init(updateLocation, prerelease);
        }

        private void SubscribeToInternalEvents()
        {
            // Registering for internal plugin state event
            var internalEvents = EventHandlerController.Get<Internal.EventHandler.Internal>();
            internalEvents.OnInternalPluginState += (s, e) => OnInternalPluginState(e);
        }

        private void OnInternalPluginState(InternalPluginState e)
        {
            // to prevent multiple updates
            if (updateNotified)
            {
                return;
            }

            if (e.PluginName == this.Name && e.PluginStatus == "Registered")
            {
                // Send update event to check for update at startup
                this.eventBus.PublishEvent(this.applicationUpdateEventFactory.CreateApplicationUpdateCommandCheckLatestVersion());
                updateNotified = true;
            }
        }

        protected void Init(string updateLocation, bool prerelease)
        {
            if (string.IsNullOrEmpty(updateLocation))
            {
                Logger.Information("Auto update is disabled, no update location specified");
                return;
            }

            using var updateManager = CreateUpdateManager(updateLocation, prerelease);

            if (updateManager.IsInstalledApp)
            {
                this.updateLocation = updateLocation;
                this.prerelease = prerelease;
                this.Logger.Information("Installed application: auto update enabled");
                this.applicationUpdate.OnApplicationUpdateCommandCheckLatestVersion += async (s, e) => await CheckForAppUpdates();
                this.applicationUpdate.OnApplicationUpdateLatestVersionChanged += async (s, e) => await OnApplicationVersionChanged();

                // Send update event to check for update at startup
                this.eventBus.PublishEvent(this.applicationUpdateEventFactory.CreateApplicationUpdateCommandCheckLatestVersion());
            }
        }

        // ToDo: figure out how to tidy this up and not mix sync and async in the same method
        private static UpdateManager CreateUpdateManager(string updateLocation, bool prerelease)
        {
            if (string.IsNullOrEmpty(updateLocation))
            {
                return null;
            }

            var isGitHub = updateLocation.StartsWith("https://github.com");

            if (isGitHub)
            {
                var asyncUpdateManager = UpdateManager.GitHubUpdateManager(updateLocation, prerelease: prerelease);
                asyncUpdateManager.Wait();
                return asyncUpdateManager.Result;
            }

            return new UpdateManager(updateLocation);
        }

        private async Task CheckForAppUpdates()
        {
            Logger.Information("Auto update, checking lastest version");
            using var updateManager = CreateUpdateManager(updateLocation, prerelease);

            var canUpdate = await updateManager.CheckForUpdate();

            if (canUpdate.ReleasesToApply.Any())
            {
                Logger.Information("Auto update, new version available, raising the event");
                this.eventBus.PublishEvent(this.applicationUpdateEventFactory.CreateApplicationUpdateLatestVersionChanged(canUpdate.FutureReleaseEntry.Version.ToString()));
            }
            else
            {
                Logger.Information("Auto update, no update available");
            }
        }

        private async Task OnApplicationVersionChanged()
        {
            using var updateManager = CreateUpdateManager(updateLocation, prerelease);
            await DoAppUpdates(updateManager);
        }

        private async Task DoAppUpdates(UpdateManager updateManager)
        {
            Logger.Information("Auto updating to the latest version");
            var releaseInfo = await updateManager.UpdateApp();
            Logger.Information($"Auto update, update completed for {releaseInfo.Version}");
        }

        public IEnumerable<ILuaGlue> CreateLuaGlues()
        {
            return new ILuaGlue[] { };
        }
    }
}