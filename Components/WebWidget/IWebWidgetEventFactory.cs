using Slipstream.Components.WebWidget.Events;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.WebWidget
{
    public interface IWebWidgetEventFactory
    {
        WebWidgetCommandEvent CreateWebWidgetCommandEvent(IEventEnvelope envelope, string data);
    }
}