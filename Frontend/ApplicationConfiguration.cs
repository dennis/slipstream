using Slipstream.Shared;

namespace Slipstream.Frontend
{
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        private readonly Properties.Settings Settings;

        public ApplicationConfiguration()
        {
            Settings = Properties.Settings.Default;

            System.IO.Directory.CreateDirectory(AudioPath);
            System.IO.Directory.CreateDirectory(ScriptPath);
        }

        public string TxrxIpPort => Settings.TxrxIpPort;
        public string AudioPath => @"Audio\";
        public string ScriptPath => @"Scripts\";
        public string[] FileMonitorPaths => new string[] { ScriptPath };
        public string TwitchUsername => Settings.TwitchUsername;
        public string TwitchChannel => Settings.TwitchChannel;
        public string TwitchToken => Settings.TwitchToken;
        public bool TwitchLog => Settings.TwitchLog;
    }
}
