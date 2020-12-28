#nullable enable

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

    public class PluginState : IEvent
    {
        public string EventType => "PluginState";
        public string Id { get; set; } = "INVALID-PLUGIN-ID";
        public string PluginName { get; set; } = "INVALID-PLUGIN-NAME";
        public string DisplayName { get; set; } = "INVALID-DISPLAY-NAME";
        public PluginStatus PluginStatus { get; set; }
    }
}
