#nullable enable

using System;

namespace Slipstream.Shared.Events.Internal
{
    public class PluginRegister : IEvent
    {
        public string EventType => "PluginRegister";
        public Guid Id { get; set; }
        public string? PluginName { get; set; }
        public bool? Enabled { get; set; }
        public IEvent? Settings { get; set; }

        public PluginRegister()
        {
            Id = Guid.NewGuid();
        }
    }
}
