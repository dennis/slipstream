using Slipstream.Components.IRacing.Plugins.Models;

#nullable enable

namespace Slipstream.Components.IRacing.Plugins.Trackers
{
    internal interface IIRacingDataTracker
    {
        public void Handle(GameState.IState currentState, IRacingDataTrackerState state);
    }
}