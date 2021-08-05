#nullable enable

using Slipstream.Shared;
using Slipstream.Shared.Lua;
using System.Threading;
using AutoUpdaterDotNET;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Runtime.Versioning;
using Slipstream.Components.Internal;

namespace Slipstream.Components.AppilcationUpdate.Lua
{
#if WINDOWS7_0_OR_GREATER

    [SupportedOSPlatform("windows7.0")]
    public class ApplicationUpdateInstanceThread : BaseInstanceThread, IApplicationUpdateInstanceThread
    {
        private readonly IEventHandlerController EventHandlerController;
        private readonly IApplicationUpdateEventFactory ApplicationUpdateEventFactory;
        private readonly IEventBusSubscription Subscription;
        private readonly IApplicationVersionService ApplicationVersionService;
        private readonly string UpdateLocation;
        private readonly bool Prerelease;
        private UpdateInfoEventArgs? LastUpdateInfoEventArgs;

        public ApplicationUpdateInstanceThread(
            string luaLibraryName,
            string instanceId,
            string location,
            bool prerelease,
            Serilog.ILogger logger,
            IEventHandlerController eventHandlerController,
            IApplicationUpdateEventFactory applicationUpdateEventFactory,
            IEventBus eventBus,
            IEventBusSubscription subscription,
            IApplicationVersionService applicationVersionService,
            IInternalEventFactory internalEventFactory
        ) : base(luaLibraryName, instanceId, logger, eventHandlerController, eventBus, internalEventFactory)
        {
            UpdateLocation = location;
            Prerelease = prerelease;
            EventHandlerController = eventHandlerController;
            ApplicationUpdateEventFactory = applicationUpdateEventFactory;
            EventBus = eventBus;
            Subscription = subscription;
            ApplicationVersionService = applicationVersionService;
        }

        private void AutoUpdaterOnParseUpdateInfoEvent(ParseUpdateInfoEventArgs args)
        {
            // We're expecting a github endpoint, returning a JSON of what is available on the release page.
            // see https://api.github.com/repos/dennis/slipstream/releases
            // We will always use the latest version and check if that differs from the installed version
            // When looking for which asset to download, we'll just download the first .exe file we find.

            // AutoUpdaterDotNET also features UI, giving a dialog box that asks user if they want to update,
            // skip or remind later. But to utilize this, we need to be doing this from the UI thread.
            // It might make sense to move it to WinForm?

            if (!(JsonConvert.DeserializeObject(args.RemoteData) is JArray json))
            {
                Logger.Information("Auto update, can't read json returned from update server");
                return;
            }

            static string GetVersionFromTag(string tag) => tag.Remove(0, tag.IndexOf('v') + 1);

            var latestRelease = json[0];
            if (latestRelease == null)
            {
                Logger.Information("Auto update, didn't find any information for last version");
                return;
            }

            if (!(latestRelease["tag_name"] is JValue newestRelease))
            {
                Logger.Information("Auto update, can't read newest version information");
                return;
            }

            var newestReleaseStr = newestRelease.Value as string;
            var CurrentVersion = GetVersionFromTag(newestReleaseStr!);
            var ChangelogUrl = string.Empty;

            if (!(latestRelease["assets"] is JArray assets))
            {
                Logger.Information("Auto update, no assets found");
                return;
            }

            foreach (var asset in assets)
            {
                if (asset["browser_download_url"] is JValue downloadUrl && downloadUrl.Value is string downloadUrlStr && downloadUrlStr.EndsWith(".exe"))
                {
                    args.UpdateInfo = new UpdateInfoEventArgs
                    {
                        CurrentVersion = CurrentVersion,
                        ChangelogURL = ChangelogUrl,
                        DownloadURL = downloadUrlStr
                    };

                    break;
                }
            }
        }

        protected override void Main()
        {
            Logger.Information($"Auto update, updating from {UpdateLocation}, prerelease: {Prerelease} {Thread.CurrentThread.ManagedThreadId}");

            AutoUpdater.HttpUserAgent = $"{Assembly.GetExecutingAssembly().GetName().Name} v{ApplicationVersionService.Version}";
            AutoUpdater.ParseUpdateInfoEvent += AutoUpdaterOnParseUpdateInfoEvent;
            AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckFOrUpdateEvent;
            AutoUpdater.PersistenceProvider = new JsonFilePersistenceProvider(Path.Combine(Environment.CurrentDirectory, "autoupdate.json"));

            var applicationUpdate = EventHandlerController.Get<EventHandler.ApplicationUpdateEventHandler>();
            applicationUpdate.OnApplicationUpdateCommandCheckLatestVersion += (s, e) => CheckForAppUpdates();
            applicationUpdate.OnApplicationUpdateLatestVersionChanged += (s, e) => OnApplicationVersionChanged();

            while (!Stopping)
            {
                IEvent? @event = Subscription.NextEvent(100);

                if (@event != null)
                {
                    EventHandlerController.HandleEvent(@event);
                }
            }
        }

        private void OnApplicationVersionChanged()
        {
            if (LastUpdateInfoEventArgs == null)
                return;

            try
            {
                if (AutoUpdater.DownloadUpdate(LastUpdateInfoEventArgs))
                {
                    Logger.Information("Auto update: Updated version. Restart to use it");
                }
                else
                {
                    Logger.Information("Auto update cancelled");
                }
            }
            catch (Exception exception)
            {
                Logger.Error("Auto update error: " + exception.Message);
            }

            LastUpdateInfoEventArgs = null;
        }

        private void AutoUpdaterOnCheckFOrUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args.Error == null)
            {
                if (args.IsUpdateAvailable)
                {
                    Logger.Information($"Auto update, new version {args.CurrentVersion} available. You are using version {args.InstalledVersion}.");

                    LastUpdateInfoEventArgs = args;

                    EventBus.PublishEvent(ApplicationUpdateEventFactory.CreateApplicationUpdateLatestVersionChanged(InstanceEnvelope, args.CurrentVersion));
                }
                else
                {
                    Logger.Information("Auto update, no update available");
                }
            }
            else
            {
                if (args.Error is WebException)
                {
                    Logger.Error("Auto update, can't reach update server");
                }
                else
                {
                    Logger.Error("Auto update error: " + args.Error.Message);
                }
            }
        }

        private void CheckForAppUpdates()
        {
            AutoUpdater.Start(UpdateLocation);
        }
    }

#endif
}