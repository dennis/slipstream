﻿#nullable enable

using Slipstream.Components.Internal;
using Slipstream.Components.Playback.Events;
using Slipstream.Shared;
using Slipstream.Shared.Lua;

using System.IO;
using System.Threading;

namespace Slipstream.Components.Playback.Lua
{
    public class PlaybackInstanceThread : BaseInstanceThread, IPlaybackInstanceThread
    {
        private readonly IEventHandlerController EventHandlerController;
        private readonly IEventSerdeService EventSerdeService;
        private readonly IEventBusSubscription Subscription;

        public PlaybackInstanceThread(
            string luaLibraryName,
            string instanceId,
            Serilog.ILogger logger,
            IEventBus eventBus,
            IEventHandlerController eventHandlerController,
            IEventSerdeService eventSerdeService,
            IEventBusSubscription eventBusSubscription,
            IInternalEventFactory internalEventFactory
        ) : base(luaLibraryName, instanceId, logger, eventHandlerController, eventBus, internalEventFactory)
        {
            EventBus = eventBus;
            EventHandlerController = eventHandlerController;
            EventSerdeService = eventSerdeService;
            Subscription = eventBusSubscription;
        }

        protected override void Main()
        {
            var playback = EventHandlerController.Get<EventHandler.Playback>();
            playback.OnPlaybackCommandInjectEvents += (s, e) => OnPlaybackCommandInjectEvents(e);
            playback.OnPlaybackCommandSaveEvents += (s, e) => OnPlaybackCommandSaveEvents(e);

            while (!Stopping)
            {
                IEvent? @event = Subscription.NextEvent(100);

                if (@event != null)
                {
                    EventHandlerController.HandleEvent(@event);
                }
            }
        }

        private void OnPlaybackCommandSaveEvents(PlaybackCommandSaveEvents @event)
        {
            var subscription = EventBus.RegisterListener(InstanceId, fromBeginning: true);

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
                            int duration = (int)(currentEvent.Envelope.Uptime - prevEvent.Envelope.Uptime);

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
    }
}