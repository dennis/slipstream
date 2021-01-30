using Slipstream.Components.Playback.Events;
using Slipstream.Shared.Factories;

#nullable enable

namespace Slipstream.Components.Playback.EventFactory
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