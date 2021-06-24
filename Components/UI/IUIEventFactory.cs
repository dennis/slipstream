using Slipstream.Components.UI.Events;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.UI
{
    public interface IUIEventFactory
    {
        UICommandWriteToConsole CreateUICommandWriteToConsole(IEventEnvelope envelope, string message);

        UICommandCreateButton CreateUICommandCreateButton(IEventEnvelope envelope, string text);

        UICommandDeleteButton CreateUICommandDeleteButton(IEventEnvelope envelope, string text);

        UIButtonTriggered CreateUIButtonTriggered(IEventEnvelope envelope, string text);
    }
}