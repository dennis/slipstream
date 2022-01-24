using Slipstream.Shared.Lua;

namespace Slipstream.Components.Lua.Lua
{
    public interface ILuaLuaLibrary : ILuaLibraryAutoRegistration
    {
        ILuaReference LoadLuaFile(string luaScriptInstanceId, string instanceId, string filePath);
    }
}