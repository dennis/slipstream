using Slipstream.Shared;
using Slipstream.Shared.Events.Setting;

namespace Slipstream.Frontend
{
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        private readonly Properties.Settings Settings;

        public ApplicationConfiguration()
        {
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
            return new Shared.Events.Setting.AudioSettings { Path = GetAudioPath() };
        }

        public FileMonitorSettings GetFileMonitorSettingsEvent()
        {
            return new Shared.Events.Setting.FileMonitorSettings { Paths = new string[] { GetScriptsPath() } };
        }

        public string GetScriptsPath()
        {
            return @"Scripts\";
        }

        public TwitchSettings GetTwitchSettingsEvent()
        {
            return new Shared.Events.Setting.TwitchSettings
            {
                TwitchUsername = Settings.TwitchUsername,
                TwitchChannel = Settings.TwitchChannel,
                TwitchToken = Settings.TwitchToken
            };
        }
    }
}
