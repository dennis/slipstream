namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingCarCompletedLap : IEvent
    {
        public string EventType => "IRacingCarCompletedLap";
        public double SessionTime { get; set; }
        public long CarIdx { get; set; }
        public double Time { get; set; }
        public int LapsComplete { get; set; }
        public float? FuelDiff { get; set; }
        public bool LocalUser { get; set; }
    }
}
