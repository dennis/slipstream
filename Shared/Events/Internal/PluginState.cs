#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class PluginState : IEvent
    {
        public string EventType => "PluginState";
        public string Id { get; set; } = "INVALID-PLUGIN-ID";
        public string PluginName { get; set; } = "INVALID-PLUGIN-NAME";
        public string DisplayName { get; set; } = "INVALID-DISPLAY-NAME";
        private string pluginStatus = "Unregistered";
        public string PluginStatus 
        {
            get { return pluginStatus; }
            set
            {

                if(value == "Registered" || value == "Unregistered" || value == "Enabled" || value == "Disabled")
                {
                    pluginStatus = value;
                }
                else
                {
                    throw new System.Exception($"Bad value for PluginStatus: '{value}'");
                }
            }
        }
    }
}
