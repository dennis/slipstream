using Slipstream.Shared;

namespace Slipstream.Components.Lua.Lua
{
    public interface ILuaLuaLibrary : ILuaLibrary
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        ILuaReference instance(string instanceId, string filePath);
    }
}