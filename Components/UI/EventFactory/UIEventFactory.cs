using Slipstream.Components.UI.Events;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.UI.EventFactory
{
    public class UIEventFactory : IUIEventFactory
    {
        public UICommandWriteToConsole CreateUICommandWriteToConsole(IEventEnvelope envelope, string message)
        {
            return new UICommandWriteToConsole
            {
                Envelope = envelope,
                Message = message
            };
        }

        public UICommandCreateButton CreateUICommandCreateButton(IEventEnvelope envelope, string text)
        {
            return new UICommandCreateButton
            {
                Envelope = envelope,
                Text = text,
            };
        }

        public UICommandDeleteButton CreateUICommandDeleteButton(IEventEnvelope envelope, string text)
        {
            return new UICommandDeleteButton
            {
                Envelope = envelope,
                Text = text,
            };
        }

        public UIButtonTriggered CreateUIButtonTriggered(IEventEnvelope envelope, string text)
        {
            return new UIButtonTriggered
            {
                Envelope = envelope,
                Text = text
            };
        }
    }
}