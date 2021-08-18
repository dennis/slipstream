using Slipstream.Components.WebWidget;
using Slipstream.Components.WebWidget.Events;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.Internal.EventFactory
{
    public class WebWidgetEventFactory : IWebWidgetEventFactory
    {
        public WebWidgetCommandEvent CreateWebWidgetCommandEvent(IEventEnvelope envelope, string data)
        {
            return new WebWidgetCommandEvent { Envelope = envelope, Data = data };
        }

        public WebWidgetEndpointAdded CreateWebWidgetEndpointAdded(IEventEnvelope envelope, string endpoint)
        {
            return new WebWidgetEndpointAdded { Envelope = envelope, Endpoint = endpoint };
        }

        public WebWidgetEndpointRemoved CreateWebWidgetEndpointRemoved(IEventEnvelope envelope, string endpoint)
        {
            return new WebWidgetEndpointRemoved { Envelope = envelope, Endpoint = endpoint };
        }
    }
}