using Slipstream.Shared;
using System;

#nullable enable

namespace Slipstream.Backend.Services
{
    public class TxrxService : ITxrxService
    {
        private readonly IEventSerdeService Serde;
        private string UnterminatedJson = "";

        public TxrxService(IEventSerdeService eventSerdeService)
        {
            Serde = eventSerdeService;
        }

        public string Serialize(IEvent e)
        {
            return Serde.Serialize(e);
        }

        public void Parse(string data, Action<IEvent> processEvent)
        {
            for(int pos = data.Length-1; pos > 0; pos-- )
            {
                if(data[pos] == '\n')
                {
                    string chunk = UnterminatedJson + data.Substring(0, pos);
                    UnterminatedJson = data.Substring(pos);

                    foreach(var @event in Serde.DeserializeMultiple(chunk))
                    {
                        processEvent(@event);
                    }
                    return;
                }
            }
        }
    }
}
