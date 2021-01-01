namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingCarCompletedLap : IEvent
    {
        public string EventType => "IRacingCarCompletedLap";
        public bool ExcludeFromTxrx => false;
        public double SessionTime { get; set; }
        public long CarIdx { get; set; }
        public double Time { get; set; }
        public int LapsCompleted { get; set; }
        public float? FuelDiff { get; set; }
        public bool LocalUser { get; set; }
    }
}
