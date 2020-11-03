namespace Slipstream.Shared.Events.Internal
{
    enum PluginStatus
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

    class PluginStateChanged : IEvent
    {
        public PluginStatus PluginStatus { get; internal set; }
        public string PluginName { get; internal set; } = "<UNSET PLUGINNAME>";
    }
}
