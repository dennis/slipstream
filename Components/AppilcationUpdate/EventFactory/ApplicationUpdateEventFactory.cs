using Slipstream.Components.AppilcationUpdate.Events;
using Slipstream.Shared;

namespace Slipstream.Components.AppilcationUpdate.EventFactory
{
    public class ApplicationUpdateEventFactory : IApplicationUpdateEventFactory
    {
        public ApplicationUpdateCommandCheckLatestVersion CreateApplicationUpdateCommandCheckLatestVersion(IEventEnvelope envelope)
        {
            return new ApplicationUpdateCommandCheckLatestVersion { Envelope = envelope.Clone() };
        }

        public ApplicationUpdateLatestVersionChanged CreateApplicationUpdateLatestVersionChanged(IEventEnvelope envelope, string version)
        {
            return new ApplicationUpdateLatestVersionChanged
            {
                Envelope = envelope.Clone(),
                LatestVersion = version,
            };
        }
    }
}