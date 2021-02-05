namespace Slipstream.Components.IRacing.Plugins.GameState
{
    internal interface ISession
    {
        public bool LapsLimited { get; }
        public bool TimeLimited { get; }
        public double TotalSessionTime { get; }
        public int TotalSessionLaps { get; }
    }
}