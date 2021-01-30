using Slipstream.Components.Playback.Events;

#nullable enable

namespace Slipstream.Components.Playback
{
    public interface IPlaybackEventFactory
    {
        PlaybackCommandSaveEvents CreatePlaybackCommandSaveEvents(string filename);

        PlaybackCommandInjectEvents CreatePlaybackCommandInjectEvents(string filename);
    }
}