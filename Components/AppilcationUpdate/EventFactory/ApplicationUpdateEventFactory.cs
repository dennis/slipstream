using Slipstream.Components.AppilcationUpdate.Events;

namespace Slipstream.Components.AppilcationUpdate.EventFactory
{
    public class ApplicationUpdateEventFactory : IApplicationUpdateEventFactory
    {
        public ApplicationUpdateCommandCheckLatestVersion CreateApplicationUpdateCommandCheckLatestVersion()
        {
            return new ApplicationUpdateCommandCheckLatestVersion();
        }

        public ApplicationUpdateLatestVersionChanged CreateApplicationUpdateLatestVersionChanged(string version)
        {
            return new ApplicationUpdateLatestVersionChanged
            {
                LatestVersion = version
            };
        }
    }
}
