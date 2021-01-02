namespace Slipstream.Shared.Events.UI
{
    public class UIButtonTriggered : IEvent
    {
        public string EventType => "UIButtonTriggered";
        public bool ExcludeFromTxrx => false;
        public string Text { get; set; } = "INVALID-NAME";
    }
}
