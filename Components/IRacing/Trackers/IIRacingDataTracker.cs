using Slipstream.Components.IRacing.Models;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.IRacing.Trackers
{
    internal interface IIRacingDataTracker
    {
        public void Handle(GameState.IState currentState, IRacingDataTrackerState state, IEventEnvelope envelope);
    }
}