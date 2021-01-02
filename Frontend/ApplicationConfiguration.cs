using Slipstream.Shared;
using Slipstream.Shared.Events.Setting;

namespace Slipstream.Frontend
{
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        private readonly IEventFactory EventFactory;
        private readonly Properties.Settings Settings;

        public ApplicationConfiguration(IEventFactory eventFactory)
        {
            EventFactory = eventFactory;
            Settings = Properties.Settings.Default;

            System.IO.Directory.CreateDirectory(GetAudioPath());
            System.IO.Directory.CreateDirectory(GetScriptsPath());
        }

        public string GetAudioPath()
        {
            return @"Audio\";
        }

        public AudioSettings GetAudioSettingsEvent()
        {
            return EventFactory.CreateAudioSettings(GetAudioPath());
        }

        public FileMonitorSettings GetFileMonitorSettingsEvent()
        {
            return EventFactory.CreateFileMonitorSettings(new string[] { GetScriptsPath() });
        }

        public string GetScriptsPath()
        {
            return @"Scripts\";
        }

        public TwitchSettings GetTwitchSettingsEvent()
        {
            return EventFactory.CreateTwitchSettings
            (
                twitchUsername: Settings.TwitchUsername,
                twitchChannel: Settings.TwitchChannel,
                twitchToken: Settings.TwitchToken,
                twitchLog: Settings.TwitchLog
            );
        }

        public TxrxSettings GetTxrxSettingsEvent()
        {
            return EventFactory.CreateTxrxSettings(Settings.TxrxIpPort);
        }
    }
}
