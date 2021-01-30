using Slipstream.Backend;
using Slipstream.Backend.Services;
using Slipstream.Components.Txrx.Plugins;
using Slipstream.Components.Txrx.Services;
using Slipstream.Shared.Factories;

namespace Slipstream.Components.Audio
{
    internal class Txrx : IComponent
    {
        public void Register(IComponentRegistrationContext ctx)
        {
            ctx.RegisterPlugin("TransmitterPlugin", CreateTransmitterPlugin);
            ctx.RegisterPlugin("ReceiverPlugin", CreateReceiverPlugin);
        }

        private IPlugin CreateReceiverPlugin(IComponentPluginCreationContext ctx)
        {
            var txrxService = new TxrxService(new EventSerdeService());

            return new ReceiverPlugin(
                ctx.EventHandlerController,
                ctx.PluginId,
                ctx.Logger,
                ctx.EventFactory.Get<IInternalEventFactory>(),
                ctx.EventBus,
                txrxService,
                ctx.PluginParameters
            );
        }

        private IPlugin CreateTransmitterPlugin(IComponentPluginCreationContext ctx)
        {
            var txrxService = new TxrxService(new EventSerdeService());

            return new TransmitterPlugin(
                ctx.EventHandlerController,
                ctx.PluginId,
                ctx.Logger,
                ctx.EventFactory.Get<IInternalEventFactory>(),
                ctx.EventBus,
                txrxService,
                ctx.PluginParameters
            );
        }
    }
}