using Slipstream.Components.Playback.Events;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.Playback.EventFactory
{
    public class PlaybackEventFactory : IPlaybackEventFactory
    {
        public PlaybackCommandInjectEvents CreatePlaybackCommandInjectEvents(IEventEnvelope envelope, string filename)
        {
            return new PlaybackCommandInjectEvents
            {
                Envelope = envelope.Clone(),
                Filename = filename
            };
        }

        public PlaybackCommandSaveEvents CreatePlaybackCommandSaveEvents(IEventEnvelope envelope, string filename)
        {
            return new PlaybackCommandSaveEvents
            {
                Envelope = envelope.Clone(),
                Filename = filename
            };
        }
    }
}