using Slipstream.Components.AppilcationUpdate.Events;
using Slipstream.Shared;

namespace Slipstream.Components.AppilcationUpdate
{
    public interface IApplicationUpdateEventFactory
    {
        ApplicationUpdateLatestVersionChanged CreateApplicationUpdateLatestVersionChanged(IEventEnvelope envelope, string version);
        ApplicationUpdateCommandCheckLatestVersion CreateApplicationUpdateCommandCheckLatestVersion(IEventEnvelope envelope);
    }
}
