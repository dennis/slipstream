#nullable enable

namespace Slipstream.Shared.Events.Internal
{
    public class LuaSettings : IEvent
    {
        public string? FilePath { get; set; }
    }
}
