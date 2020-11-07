#nullable enable

using System;

namespace Slipstream.Shared.Events.Internal
{
    public class PluginEnable : IEvent
    {
        public string EventType => "PluginEnable";

        public Guid Id { get; set; }

        public PluginEnable()
        {
            Id = Guid.NewGuid();
        }
    }
}
