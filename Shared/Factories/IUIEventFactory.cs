using Slipstream.Shared.Events.UI;

#nullable enable

namespace Slipstream.Shared.Factories
{
    public interface IUIEventFactory
    {
        UICommandWriteToConsole CreateUICommandWriteToConsole(string message);
        UICommandCreateButton CreateUICommandCreateButton(string text);
        UICommandDeleteButton CreateUICommandDeleteButton(string text);
        UIButtonTriggered CreateUIButtonTriggered(string text);
    }
}
