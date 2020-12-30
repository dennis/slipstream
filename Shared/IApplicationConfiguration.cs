namespace Slipstream.Shared
{
    interface IApplicationConfiguration
    {
        public Events.Setting.TwitchSettings GetTwitchSettingsEvent();
        public Events.Setting.FileMonitorSettings GetFileMonitorSettingsEvent();
        public Events.Setting.AudioSettings GetAudioSettingsEvent();
        public Events.Setting.TxrxSettings GetTxrxSettingsEvent();
    }
}
