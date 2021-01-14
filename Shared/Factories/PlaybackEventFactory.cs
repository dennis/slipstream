using Slipstream.Shared.Events.Playback;

#nullable enable

namespace Slipstream.Shared.Factories
{
    public class PlaybackEventFactory : IPlaybackEventFactory
    {
        public PlaybackCommandInjectEvents CreatePlaybackCommandInjectEvents(string filename)
        {
            return new PlaybackCommandInjectEvents
            {
                Filename = filename
            };
        }

        public PlaybackCommandSaveEvents CreatePlaybackCommandSaveEvents(string filename)
        {
            return new PlaybackCommandSaveEvents
            {
                Filename = filename
            };
        }
    }
}
