using Slipstream.Shared;

namespace Slipstream.Components.AppilcationUpdate.Events
{
    public class ApplicationUpdateLatestVersionChanged : IEvent
    {
        public string EventType => nameof(ApplicationUpdateLatestVersionChanged);

        public bool ExcludeFromTxrx => true;

        public ulong Uptime { get; set; }

        public string LatestVersion { get; set; }
    }
}
