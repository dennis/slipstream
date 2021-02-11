namespace Slipstream.Components.IRacing.Plugins.GameState
{
    internal class Session : ISession
    {
        public bool LapsLimited { get; set; }
        public bool TimeLimited { get; set; }
        public double TotalSessionTime { get; set; }
        public int TotalSessionLaps { get; set; }
    }
}