using Serilog;
using Slipstream.Components.Internal;
using Slipstream.Shared;

namespace Slipstream.Components
{
    internal interface IComponentPluginDependencies
    {
        IEventHandlerController EventHandlerController { get; }
        ILogger Logger { get; }
        IEventBus EventBus { get; }
        IEventFactory EventFactory { get; }
        IServiceLocator ServiceLocator { get; }
    }
}