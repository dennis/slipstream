#nullable enable

using Slipstream.Shared;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.Audio.Lua
{
    public class AudioLuaReference : ILuaReference
    {
        private readonly string InstanceId;
        private readonly IEventBus EventBus;
        private readonly IAudioEventFactory EventFactory;

        private float Volume = 1.0f;

        public AudioLuaReference(string instanceId, IEventBus eventBus, IAudioEventFactory eventFactory)
        {
            InstanceId = instanceId;
            EventBus = eventBus;
            EventFactory = eventFactory;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void say(string message)
        {
            EventBus.PublishEvent(EventFactory.CreateAudioCommandSay(InstanceId, message, Volume));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void play(string filename)
        {
            EventBus.PublishEvent(EventFactory.CreateAudioCommandPlay(InstanceId, filename, Volume));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void send_devices()
        {
            EventBus.PublishEvent(EventFactory.CreateAudioCommandSendDevices(InstanceId));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void set_output(int deviceIdx)
        {
            EventBus.PublishEvent(EventFactory.CreateAudioCommandSetOutputDevice(InstanceId, deviceIdx));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void set_volume(float volume)
        {
            Volume = volume;
        }

        public void Dispose()
        {
        }
    }
}