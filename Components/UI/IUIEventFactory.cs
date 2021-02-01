using Slipstream.Components.UI.Events;

#nullable enable

namespace Slipstream.Components.UI
{
    public interface IUIEventFactory
    {
        UICommandWriteToConsole CreateUICommandWriteToConsole(string message);

        UICommandCreateButton CreateUICommandCreateButton(string text);

        UICommandDeleteButton CreateUICommandDeleteButton(string text);

        UIButtonTriggered CreateUIButtonTriggered(string text);
    }
}