#nullable enable

using iRacingSDK;
using System;
using System.Diagnostics;
using System.Linq;

namespace Slipstream.Components.IRacing.Plugins.GameState
{
    internal class Mapper
    {
        private readonly iRacingConnection Connection = new iRacingConnection();

        internal IState? GetState()
        {
            try
            {
                return new State(Connection.GetDataFeed().WithCorrectedDistances().WithCorrectedPercentages().First());
            }
            catch (Exception e) when (e.Message == "Attempt to read session data before connection to iRacing" || e.Message == "Attempt to read telemetry data before connection to iRacing")
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);

                return null;
            }
        }
    }
}