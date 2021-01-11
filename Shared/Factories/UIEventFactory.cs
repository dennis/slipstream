using Slipstream.Shared.Events.UI;

#nullable enable

namespace Slipstream.Shared.Factories
{
    public class UIEventFactory : IUIEventFactory
    {
        public UICommandWriteToConsole CreateUICommandWriteToConsole(string message)
        {
            return new UICommandWriteToConsole
            {
                Message = message
            };
        }

        public UICommandCreateButton CreateUICommandCreateButton(string text)
        {
            return new UICommandCreateButton
            {
                Text = text,
            };
        }

        public UICommandDeleteButton CreateUICommandDeleteButton(string text)
        {
            return new UICommandDeleteButton
            {
                Text = text,
            };
        }

        public UIButtonTriggered CreateUIButtonTriggered(string text)
        {
            return new UIButtonTriggered
            {
                Text = text
            };
        }
    }
}
