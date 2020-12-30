namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingDisconnected : IEvent
    {
        public string EventType => "IRacingDisconnected";
        public bool ExcludeFromTxrx => false;
    }
}
