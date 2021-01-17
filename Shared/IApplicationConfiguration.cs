namespace Slipstream.Shared
{
    public interface IApplicationConfiguration : ITxrxConfiguration, IAudioConfiguration, IFileMonitorConfiguration, ITwitchConfiguration
    {
    }

    public interface ITxrxConfiguration
    {
        string TxrxIpPort { get; }
    }

    public interface IAudioConfiguration
    {
        string AudioPath { get; }
    }

    public interface IFileMonitorConfiguration
    {
        string[] FileMonitorPaths { get; }
    }

    public interface ITwitchConfiguration
    {
        string TwitchUsername { get; }
        string TwitchChannel { get; }
        string TwitchToken { get; }
        bool TwitchLog { get; }
    }
}