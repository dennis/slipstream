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
            int driverIncidents = currentState.DriverIncidentCount;
            var driverIncidentDelta = driverIncidents - state.DriverState_.DriverIncidentCount;

            int teamIncidents = currentState.TeamIncidentCount;
            var teamIncidentsDelta = teamIncidents - state.DriverState_.TeamIncidentCount;

            int myIncidents = currentState.MyIncidentCount;
            var myIncidentsDelta = myIncidents - state.DriverState_.MyIncidentCount;

            if (driverIncidentDelta + teamIncidentsDelta + myIncidentsDelta > 0)
            {
                state.DriverState_.DriverIncidentCount = driverIncidents;
                state.DriverState_.TeamIncidentCount = teamIncidents;
                state.DriverState_.MyIncidentCount = myIncidents;

                EventBus.PublishEvent(EventFactory.CreateIRacingDriverIncident(
                    driverIncidents: driverIncidents,
                    driverIncidentsDelta: driverIncidentDelta,
                    teamIncidents: teamIncidents,
                    teamIncidentsDelta: teamIncidentsDelta,
                    myIncidents: myIncidents,
                    myIncidentsDelta: myIncidentsDelta
                ));
            }
        }
    }
}