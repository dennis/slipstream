using Newtonsoft.Json;
using Slipstream.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private class Meta : IEvent
        {
            public string EventType { get; set; } = String.Empty;

            public bool ExcludeFromTxrx { get; set; }
        }

        public IEvent? Deserialize(string json)
        {
            var meta = JsonConvert.DeserializeObject(json, typeof(Meta)) as Meta;
            var eventType = meta?.EventType;

            if (eventType != null && EventsMap.ContainsKey(eventType))
            {
                var obj = JsonConvert.DeserializeObject(json, EventsMap[eventType!]);

                if (obj != null)
                {
                    return (IEvent)obj;
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
            return JsonConvert.SerializeObject(@event) + "\n";
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
