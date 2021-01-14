using Serilog;
using Slipstream.Backend.Services;
using Slipstream.Shared;
using Slipstream.Shared.EventHandlers;
using Slipstream.Shared.Events.Playback;
using System.IO;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    internal class PlaybackPlugin : BasePlugin
    {
        private readonly ILogger Logger;
        private readonly IEventBus EventBus;
        private readonly IEventSerdeService EventSerdeService;

        public PlaybackPlugin(string id, ILogger logger, IEventBus eventBus, IEventSerdeService eventSerdeService) : base(id, "AudioPlugin", "AudioPlugin", "Audio", true)
        {
            EventBus = eventBus;
            EventSerdeService = eventSerdeService;
            var playback = EventHandler.Get<Playback>();

            playback.OnPlaybackInjectEvents += (s, e) => OnPlaybackInjectEvents(e.Event);
            playback.OnPlaybackSaveEvents += (s, e) => OnPlaybackSaveEvents(e.Event);

            Logger = logger;
        }

        private void OnPlaybackSaveEvents(PlaybackSaveEvents @event)
        {
            var subscription = EventBus.RegisterListener(fromBeginning: true);

            using var streamWriter = new StreamWriter(@event.Filename)
            {
                AutoFlush = true
            };

            IEvent? e;

            while((e = subscription.NextEvent(0)) != null)
            {
                string json = EventSerdeService.Serialize(e);

                streamWriter.Write(json);
            }

            Logger.Information($"Events saved to: {@event.Filename}");
        }

        private void OnPlaybackInjectEvents(PlaybackInjectEvents @event)
        {
            string json = File.ReadAllText(@event.Filename);

            foreach (var e in EventSerdeService.DeserializeMultiple(json))
            {
                if (e.EventType != "PlaybackInjectEvents" && e.EventType != "PlaybackSaveEvents")
                {
                    EventBus.PublishEvent(e);
                }
            }
        }
    }
}