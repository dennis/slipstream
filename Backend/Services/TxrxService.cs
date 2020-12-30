using Slipstream.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;

#nullable enable

namespace Slipstream.Backend.Services
{
    public class TxrxService
    {
        class JsonSerde
        {
            private static readonly Dictionary<string, Type> EventsMap = new Dictionary<string, Type>();

            static JsonSerde()
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

        private readonly JsonSerde Serde = new JsonSerde();
        private string UnterminatedJson = "";

        public string Serialize(IEvent e)
        {
            string json = Serde.Serialize(e);

            Debug.Assert(!json.Contains("\n"));

            return json + "\n";
        }

        public void Parse(string data, Action<IEvent> processLine)
        {
            int consumedPos = 0;
            int pos;
            for (pos = 0; pos < data.Length; pos++)
            {
                if (data[pos] == '\n')
                {
                    string line = UnterminatedJson + data.Substring(consumedPos, pos - consumedPos);
                    consumedPos = pos + 1; // We want to skip \n
                    UnterminatedJson = String.Empty;

                    IEvent? e = Serde.Deserialize(line);
                    if (e != null)
                    {
                        processLine(e);
                    }
                }
            }

            if (consumedPos < pos)
            {
                UnterminatedJson += data.Substring(consumedPos);
            }
        }
    }
}
