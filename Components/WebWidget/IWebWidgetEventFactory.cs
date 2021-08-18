using Slipstream.Components.WebWidget.Events;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.WebWidget
{
    public interface IWebWidgetEventFactory
    {
        WebWidgetCommandEvent CreateWebWidgetCommandEvent(IEventEnvelope envelope, string data);

        WebWidgetEndpointAdded CreateWebWidgetEndpointAdded(IEventEnvelope envelope, string endpoint);

        WebWidgetEndpointRemoved CreateWebWidgetEndpointRemoved(IEventEnvelope envelope, string endpoint);
    }
}