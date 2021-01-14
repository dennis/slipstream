using Slipstream.Shared.Events.Playback;

#nullable enable

namespace Slipstream.Shared.Factories
{
    public interface IPlaybackEventFactory
    {
        PlaybackCommandSaveEvents CreatePlaybackCommandSaveEvents(string filename);
        PlaybackCommandInjectEvents CreatePlaybackCommandInjectEvents(string filename);
    }
}
