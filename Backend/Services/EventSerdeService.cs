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
            if(JsonDocument.Parse(json).RootElement.TryGetProperty("EventType", out JsonElement eventTypeProp))
            {
                var eventType = eventTypeProp.GetString();

                if (eventType != null && EventsMap.ContainsKey(eventType))
                {
                    json = json.Replace(@"\n", "\n");

                    var obj = JsonSerializer.Deserialize(json, EventsMap[eventType!]);

                    if (obj != null)
                    {
                        return (IEvent)obj;
                    }
                }
            }

            return null;
        }

        public IEvent[] DeserializeMultiple(string json)
        {
            var result = new List<IEvent>();

            foreach(var line in json.Split('\n'))
            {
                if(line.Length > 0)
                {
                    var @event = Deserialize(line);

                    if(@event != null)
                    {
                        result.Add(@event);
                    }
                }
            }

            return result.ToArray();
        }

        public string Serialize(IEvent @event)
        {
            var json = JsonSerializer.Serialize(@event, EventsMap[@event.EventType], null);

            return json.Replace("\n", @"\n") + "\n";
        }

        public string SerializeMultiple(IEvent[] events)
        {
            string result = "";
            foreach(var @event in events)
            {
                result += Serialize(@event);
            }

            return result;
        }
    }
}
