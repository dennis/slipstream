using Slipstream.Shared;
using System;

#nullable enable

namespace Slipstream.Components.Txrx
{
    public interface ITxrxService
    {
        string Serialize(IEvent e);

        void Parse(string data, Action<IEvent> processLine);
    }
}