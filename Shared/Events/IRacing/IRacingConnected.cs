namespace Slipstream.Shared.Events.IRacing
{
    public class IRacingConnected : IEvent
    {
        public string EventType => "IRacingConnected";
        public bool ExcludeFromTxrx => false;
    }
}
