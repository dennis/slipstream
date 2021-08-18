using Slipstream.Components.WinFormUI.Events;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.WinFormUI
{
    public interface IWinFormUIEventFactory
    {
        WinFormUICommandWriteToConsole CreateWinFormUICommandWriteToConsole(IEventEnvelope envelope, string message, bool error);

        WinFormUICommandCreateButton CreateWinFormUICommandCreateButton(IEventEnvelope envelope, string text);

        WinFormUICommandDeleteButton CreateWinFormUICommandDeleteButton(IEventEnvelope envelope, string text);

        WinFormUIButtonTriggered CreateWinFormUIButtonTriggered(IEventEnvelope envelope, string text);
    }
}