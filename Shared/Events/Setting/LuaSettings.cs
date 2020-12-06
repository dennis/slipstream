#nullable enable

namespace Slipstream.Shared.Events.Setting
{
    public class LuaSettings : IEvent
    {
        public string EventType => "LuaSettings";
        public string PluginId { get; set; } = "INVALID-PLUGIN-ID";
        public string FilePath { get; set; } = string.Empty;
    }
}
