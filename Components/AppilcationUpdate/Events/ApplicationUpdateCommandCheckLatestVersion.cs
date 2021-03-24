using Slipstream.Shared;

namespace Slipstream.Components.AppilcationUpdate.Events
{
    public class ApplicationUpdateCommandCheckLatestVersion : IEvent
    {
        public string EventType => nameof(ApplicationUpdateCommandCheckLatestVersion);

        public bool ExcludeFromTxrx => true;

        public ulong Uptime { get; set; }
    }
}
