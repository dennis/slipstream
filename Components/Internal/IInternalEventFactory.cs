using Slipstream.Components.Internal.Events;

#nullable enable

namespace Slipstream.Components.Internal
{
    public interface IInternalEventFactory
    {
        InternalCommandShutdown CreateInternalCommandShutdown();
    }
}