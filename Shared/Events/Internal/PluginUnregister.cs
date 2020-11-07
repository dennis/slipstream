#nullable enable

using System;

namespace Slipstream.Shared.Events.Internal
{
    public class PluginUnregister : IEvent
    {
        public string EventType => "PluginUnregister";
        public Guid Id { get; set; }

        public PluginUnregister()
        {
            Id = Guid.NewGuid();
        }
    }
}
