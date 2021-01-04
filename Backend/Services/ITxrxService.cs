using Slipstream.Shared;
using System;

#nullable enable

namespace Slipstream.Backend.Services
{
    public interface ITxrxService
    {
        string Serialize(IEvent e);
        void Parse(string data, Action<IEvent> processLine);
    }
}
