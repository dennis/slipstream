namespace Slipstream.Shared.Events.IRacing
{
    public interface IIRacingSessionState : IEvent
    {
        double SessionTime { get; set; }
        bool LapsLimited { get; set; }
        bool TimeLimited { get; set; }
        double TotalSessionTime { get; set; }
        int TotalSessionLaps { get; set; }
        string State { get; set; } // Checkered, CoolDown, GetInChar, ParadeLaps, Racing, Warmup
        string Category { get; set; } // Road, Oval, DirtRoad, DirtOval
    }
}