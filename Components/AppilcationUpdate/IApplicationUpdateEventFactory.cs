using Slipstream.Components.AppilcationUpdate.Events;

namespace Slipstream.Components.AppilcationUpdate
{
    public interface IApplicationUpdateEventFactory
    {
        ApplicationUpdateLatestVersionChanged CreateApplicationUpdateLatestVersionChanged(string version);
        ApplicationUpdateCommandCheckLatestVersion CreateApplicationUpdateCommandCheckLatestVersion();
    }
}
