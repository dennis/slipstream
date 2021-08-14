using Slipstream.Components.WinFormUI.Events;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.WinFormUI.EventFactory
{
    public class WinFormUIEventFactory : IWinFormUIEventFactory
    {
        public WinFormUICommandWriteToConsole CreateWinFormUICommandWriteToConsole(IEventEnvelope envelope, string message, bool error)
        {
            return new WinFormUICommandWriteToConsole
            {
                Envelope = envelope,
                Message = message,
                Error = error
            };
        }

        public WinFormUICommandCreateButton CreateWinFormUICommandCreateButton(IEventEnvelope envelope, string text)
        {
            return new WinFormUICommandCreateButton
            {
                Envelope = envelope,
                Text = text,
            };
        }

        public WinFormUICommandDeleteButton CreateWinFormUICommandDeleteButton(IEventEnvelope envelope, string text)
        {
            return new WinFormUICommandDeleteButton
            {
                Envelope = envelope,
                Text = text,
            };
        }

        public WinFormUIButtonTriggered CreateWinFormUIButtonTriggered(IEventEnvelope envelope, string text)
        {
            return new WinFormUIButtonTriggered
            {
                Envelope = envelope,
                Text = text
            };
        }
    }
}