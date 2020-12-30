namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingPitEnter : IEvent
    {
        public string EventType => "IRacingPitEnter";
        public bool ExcludeFromTxrx => false;
        public double SessionTime { get; set; }
        public long CarIdx { get; set; }
        public bool LocalUser { get; set; }
    }
}
