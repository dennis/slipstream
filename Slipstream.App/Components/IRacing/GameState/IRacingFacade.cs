#nullable enable

using iRacingSDK;
using System;
using System.Diagnostics;
using System.Linq;

namespace Slipstream.Components.IRacing.GameState
{
    internal class IRacingFacade
    {
        private readonly iRacingConnection Connection = new iRacingConnection();
        private readonly IStateFactory StateFactory;

        public IRacingFacade(IStateFactory stateFactory)
        {
            StateFactory = stateFactory;
        }

        internal IState? GetState()
        {
            try
            {
                return StateFactory.BuildState(Connection.GetDataFeed().WithCorrectedDistances().WithCorrectedPercentages().First());
            }
            catch (Exception e) when (e.Message == "Attempt to read session data before connection to iRacing" || e.Message == "Attempt to read telemetry data before connection to iRacing")
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);

                return null;
            }
        }

        internal void PitCleanWindshield()
        {
            Connection.PitCommand.CleanWindshield();
        }

        internal void PitChangeRightRearTyre(int kpa)
        {
            Connection.PitCommand.ChangeRightRearTire(kpa);
        }

        internal void PitChangeLeftRearTyre(int kpa)
        {
            Connection.PitCommand.ChangeLeftRearTire(kpa);
        }

        internal void PitChangeRightFrontTyre(int kpa)
        {
            Connection.PitCommand.ChangeRightFrontTire(kpa);
        }

        internal void PitChangeLeftFrontTyre(int kpa)
        {
            Connection.PitCommand.ChangeLeftFrontTire(kpa);
        }

        internal void PitAddFuel(int addLiters)
        {
            Connection.PitCommand.SetFuel(addLiters);
        }

        internal void PitRequestFastRepair()
        {
            Connection.PitCommand.RequestFastRepair();
        }

        internal void PitClearTyreChange()
        {
            Connection.PitCommand.ClearTireChange();
        }

        internal void PitClearAll()
        {
            Connection.PitCommand.Clear();
        }
    }
}