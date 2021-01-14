using Slipstream.Shared.Events.Playback;

#nullable enable

namespace Slipstream.Shared.Factories
{
    public interface IPlaybackEventFactory
    {
        PlaybackSaveEvents CreatePlaybackSaveEvents(string filename);
        PlaybackInjectEvents CreatePlaybackInjectEvents(string filename);
    }
}
