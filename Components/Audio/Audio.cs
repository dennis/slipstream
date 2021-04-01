using Slipstream.Backend;
using Slipstream.Components.Audio.Plugins;

namespace Slipstream.Components.Audio
{
    internal class Audio : IComponent
    {
        private const string NAME = "AudioPlugin";

        public void Register(IComponentRegistrationContext ctx)
        {
            var eventFactory = new EventFactory.AudioEventFactory();

            ctx.RegisterPlugin(NAME, CreateAudioPlugin);
            ctx.RegisterLuaGlue(new LuaGlueFactory(ctx.EventBus, eventFactory));
        }

        private IPlugin CreateAudioPlugin(IComponentPluginCreationContext ctx)
        {
            return new AudioPlugin(
                ctx.EventHandlerController,
                ctx.PluginId,
                ctx.Logger,
                ctx.EventBus,
                ctx.AudioEventFactory,
                ctx.PluginParameters
            );
        }
    }
}