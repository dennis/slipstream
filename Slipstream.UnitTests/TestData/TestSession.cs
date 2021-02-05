using Slipstream.Components.IRacing.Plugins.GameState;

namespace Slipstream.UnitTests.TestData
{
    internal partial class CarInfoDataSampleTestData
    {
        private class TestSession : ISession
        {
            public bool LapsLimited { get; set; } = true;

            public bool TimeLimited { get; set; } = false;

            public double TotalSessionTime { get; set; } = 0;

            public int TotalSessionLaps { get; set; } = 20;
        }
    }
}