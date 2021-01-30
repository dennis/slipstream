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
            ctx.RegisterEventFactory(typeof(IAudioEventFactory), eventFactory);
            ctx.RegisterEventHandler(typeof(EventHandler.AudioEventHandler));
            ctx.RegisterLuaGlue(new LuaGlue(ctx.EventBus, eventFactory));
        }

        private IPlugin CreateAudioPlugin(IComponentPluginCreationContext ctx)
        {
            return new AudioPlugin(
                ctx.EventHandlerController,
                ctx.PluginId,
                ctx.Logger,
                ctx.EventBus,
                ctx.EventFactory.Get<IAudioEventFactory>(),
                ctx.PluginParameters
            );
        }
    }
}