using Slipstream.Shared;

namespace Slipstream.Components.FileMonitor.Lua
{
    public interface IFileMonitorLuaReference : ILuaReference
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        void scan();
    }
}