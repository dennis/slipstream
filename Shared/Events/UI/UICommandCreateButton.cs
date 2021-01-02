namespace Slipstream.Shared.Events.UI
{
    public class UICommandCreateButton : IEvent
    {
        public string EventType => "UICommandCreateButton";
        public bool ExcludeFromTxrx => true;
        public string Text { get; set; } = "";
    }
}
