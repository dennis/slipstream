namespace Slipstream.Shared.Events.UI
{
    public class UICommandDeleteButton : IEvent
    {
        public string EventType => "UICommandDeleteButton";
        public bool ExcludeFromTxrx => true;
        public string Text { get; set; } = "";
    }
}
