using Slipstream.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;

#nullable enable

namespace Slipstream.Backend.Services
{
    public class EventSerdeService : IEventSerdeService
    {
        private static readonly Dictionary<string, Type> EventsMap = new Dictionary<string, Type>();

        static EventSerdeService()
        {
            foreach (var type in typeof(TxrxService).Assembly
                .GetTypes()
                .Where(t => typeof(IEvent).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass))
            {
                EventsMap.Add(type.Name, type);
            }
        }

        public IEvent? Deserialize(string json)
        {
            var eventType = JsonDocument.Parse(json).RootElement.GetProperty("EventType").GetString();

            Debug.Assert(eventType != null);

            var obj = JsonSerializer.Deserialize(json, EventsMap[eventType!]);

            if (obj != null)
            {
                return (IEvent)obj;
            }
            else
            {
                return null;
            }
        }

        public string Serialize(IEvent @event)
        {
            return JsonSerializer.Serialize(@event, EventsMap[@event.EventType], null);
        }
    }
}
