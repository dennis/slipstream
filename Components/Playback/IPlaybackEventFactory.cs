using Slipstream.Components.Playback.Events;
using Slipstream.Shared;

#nullable enable

namespace Slipstream.Components.Playback
{
    public interface IPlaybackEventFactory
    {
        PlaybackCommandSaveEvents CreatePlaybackCommandSaveEvents(IEventEnvelope envelope, string filename);

        PlaybackCommandInjectEvents CreatePlaybackCommandInjectEvents(IEventEnvelope envelope, string filename);
    }
}