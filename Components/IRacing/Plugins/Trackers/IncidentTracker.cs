using Slipstream.Components.IRacing.Plugins.Models;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.IRacing.Plugins.Trackers
{
    internal class IncidentTracker : IIRacingDataTracker
    {
        private readonly IIRacingEventFactory EventFactory;
        private readonly IEventBus EventBus;

        public IncidentTracker(IEventBus eventBus, IIRacingEventFactory eventFactory)
        {
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        public void Handle(GameState.IState currentState, IRacingDataTrackerState state)
        {
            int incidents = currentState.DriverIncidentCount;
            var incidentDelta = incidents - state.DriverState_.PlayerCarDriverIncidentCount;

            if (incidentDelta > 0)
            {
                state.DriverState_.PlayerCarDriverIncidentCount = incidents;
                EventBus.PublishEvent(EventFactory.CreateIRacingDriverIncident(totalIncidents: incidents, incidentDelta: incidentDelta));
            }
        }
    }
}