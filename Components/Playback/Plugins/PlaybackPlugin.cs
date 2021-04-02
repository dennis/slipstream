using Serilog;
using Slipstream.Components.Internal;
using Slipstream.Components.Playback.Events;
using Slipstream.Shared;
using System.Collections.Generic;
using System.IO;
using System.Threading;

#nullable enable

namespace Slipstream.Components.Playback.Plugins
{
    internal class PlaybackPlugin : BasePlugin, IPlugin
    {
        private readonly ILogger Logger;
        private readonly IEventBus EventBus;
        private readonly IEventSerdeService EventSerdeService;
        private readonly IPlaybackEventFactory EventFactory;

        public PlaybackPlugin(IEventHandlerController eventHandlerController, string id, ILogger logger, IEventBus eventBus, IPlaybackEventFactory eventFactory, IEventSerdeService eventSerdeService) : base(eventHandlerController, id, "PlaybackPlugin", id, true)
        {
            EventBus = eventBus;
            EventSerdeService = eventSerdeService;
            EventFactory = eventFactory;

            var playback = EventHandlerController.Get<EventHandler.Playback>();

            playback.OnPlaybackCommandInjectEvents += (s, e) => OnPlaybackCommandInjectEvents(e);
            playback.OnPlaybackCommandSaveEvents += (s, e) => OnPlaybackCommandSaveEvents(e);

            Logger = logger;
        }

        private void OnPlaybackCommandSaveEvents(PlaybackCommandSaveEvents @event)
        {
            var subscription = EventBus.RegisterListener(fromBeginning: true);

            using var streamWriter = new StreamWriter(@event.Filename)
            {
                AutoFlush = true
            };

            IEvent? currentEvent;

            while ((currentEvent = subscription.NextEvent(0)) != null)
            {
                string json = EventSerdeService.Serialize(currentEvent);

                streamWriter.Write(json);
            }

            Logger.Information($"Events saved to: {@event.Filename}");
        }

        private void OnPlaybackCommandInjectEvents(PlaybackCommandInjectEvents @event)
        {
            string json = File.ReadAllText(@event.Filename);

            IEvent? prevEvent = null;
            try
            {
                foreach (var currentEvent in EventSerdeService.DeserializeMultiple(json))
                {
                    if (currentEvent.EventType != "PlaybackCommandInjectEvents" && currentEvent.EventType != "PlaybackCommandSaveEvents")
                    {
                        if (prevEvent != null)
                        {
                            int duration = (int)(currentEvent.Uptime - prevEvent.Uptime);
                            if (duration > 0)
                            {
                                Thread.Sleep(duration);
                            }
                        }

                        prevEvent = currentEvent;

                        EventBus.PublishEvent(currentEvent);
                    }
                }
            }
            catch (Newtonsoft.Json.JsonReaderException e)
            {
                Logger.Warning($"Error reading {@event.Filename}: {e.Message}");
            }
        }

        public IEnumerable<ILuaGlue> CreateLuaGlues()
        {
            return new ILuaGlue[] { new LuaGlue(EventBus, EventFactory) };
        }
    }
}