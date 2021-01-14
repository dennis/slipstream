using Slipstream.Shared.Events.Playback;

#nullable enable

namespace Slipstream.Shared.Factories
{
    public class PlaybackEventFactory : IPlaybackEventFactory
    {
        public PlaybackInjectEvents CreatePlaybackInjectEvents(string filename)
        {
            return new PlaybackInjectEvents
            {
                Filename = filename
            };
        }

        public PlaybackSaveEvents CreatePlaybackSaveEvents(string filename)
        {
            return new PlaybackSaveEvents
            {
                Filename = filename
            };
        }
    }
}
