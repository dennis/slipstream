using Slipstream.Shared;
using System;
using System.Diagnostics;

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
