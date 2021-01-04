#nullable enable

namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingDriverIncident : IEvent
    {
        public string EventType => "IRacingDriverIncident";
        public bool ExcludeFromTxrx => false;

        public int IncidentCount { get; set; }

        public int IncidentDelta { get; set; }
    }
}