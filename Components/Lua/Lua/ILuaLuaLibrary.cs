using Slipstream.Shared;

namespace Slipstream.Components.Lua.Lua
{
    public interface ILuaLuaLibrary : ILuaLibrary
    {
        ILuaReference instance(string instanceId, string filePath);
    }
}