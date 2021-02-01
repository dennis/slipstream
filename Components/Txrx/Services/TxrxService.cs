using Slipstream.Components.Internal;
using Slipstream.Shared;
using System;

#nullable enable

namespace Slipstream.Components.Txrx.Services
{
    public class TxrxService : ITxrxService
    {
        public class TxrxServiceException : Exception
        {
            public TxrxServiceException() : base()
            {
            }

            public TxrxServiceException(string message) : base(message)
            {
            }

            public TxrxServiceException(string msg, Exception e) : base(msg, e)
            {
            }
        }

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
            for (int pos = data.Length - 1; pos > 0; pos--)
            {
                if (data[pos] == '\n')
                {
                    string chunk = UnterminatedJson + data.Substring(0, pos);
                    UnterminatedJson = data.Substring(pos + 1);

                    try
                    {
                        foreach (var @event in Serde.DeserializeMultiple(chunk))
                        {
                            processEvent(@event);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new TxrxServiceException("Can't parse string: " + chunk, e);
                    }

                    return;
                }
            }
        }
    }
}