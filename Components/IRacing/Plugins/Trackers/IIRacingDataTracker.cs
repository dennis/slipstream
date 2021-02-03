using iRacingSDK;
using Slipstream.Components.IRacing.Plugins.Models;

#nullable enable

namespace Slipstream.Components.IRacing.Plugins.Trackers
{
    internal interface IIRacingDataTracker
    {
        public void Handle(DataSample data, IRacingDataTrackerState state);
    }
}