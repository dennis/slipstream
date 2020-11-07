#nullable enable

using System;

namespace Slipstream.Shared.Events.Internal
{
    public enum PluginStatus
    {
        /// <summary>
        /// Plugin is registered, but not enabled/disabled (which implies the Plugin is Registered)
        /// </summary>
        Registered,
        /// <summary>
        /// Plugin is unregistered. It need to be registered to be usable
        /// </summary>
        Unregistered,
        /// <summary>
        /// Plugin is registered and fully functional
        /// </summary>
        Enabled,
        /// <summary>
        /// Plugin is registered but disabled
        /// </summary>
        Disabled
    };

    public class PluginStateChanged : IEvent
    {
        public System.Guid Id { get; set; }
        public string? PluginName { get; set; }
        public string? DisplayName { get; set; }
        public PluginStatus PluginStatus { get; internal set; }

        public PluginStateChanged()
        {
            Id = Guid.NewGuid();
        }
    }
}
