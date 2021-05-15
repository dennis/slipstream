using Slipstream.Components.IRacing.GameState;

namespace Slipstream.UnitTests.TestData
{
    public class TestSession : ISession
    {
        public bool LapsLimited { get; set; } = true;

        public bool TimeLimited { get; set; } = false;

        public double TotalSessionTime { get; set; } = 0;

        public int TotalSessionLaps { get; set; } = 20;
    }
}