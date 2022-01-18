using System;

using Serilog.Core;
using Serilog.Events;

namespace Slipstream.Shared.Logger
{
    internal class SessionIdEnricher : ILogEventEnricher
    {
        private static readonly String _guid;

        static SessionIdEnricher()
        {
            _guid = System.Guid.NewGuid().ToString();
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                    "SessionId", _guid));
        }
    }
}