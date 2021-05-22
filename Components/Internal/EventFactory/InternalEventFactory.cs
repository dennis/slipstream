using Newtonsoft.Json;
using Slipstream.Components.Internal.Events;

#nullable enable

namespace Slipstream.Components.Internal.EventFactory
{
    public class InternalEventFactory : IInternalEventFactory
    {
        public InternalCommandShutdown CreateInternalCommandShutdown()
        {
            return new InternalCommandShutdown();
        }
    }
}