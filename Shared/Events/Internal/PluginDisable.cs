#nullable enable

using System;

namespace Slipstream.Shared.Events.Internal
{
    public class PluginDisable : IEvent
    {
        public string EventType => "PluginDisable";

        public Guid Id { get; set; }

        public PluginDisable()
        {
            Id = Guid.NewGuid();
        }
    }
}
