using Slipstream.Shared.Lua;

namespace Slipstream.Components.WinFormUI.Lua
{
    public class WinFormUIReference : BaseLuaReference, IWinFormUIReference
    {
        public WinFormUILuaLibrary LuaLibrary { get; }

        public WinFormUIReference(string instanceId, string luaScriptInstanceId, WinFormUILuaLibrary luaLibrary) : base(instanceId, luaScriptInstanceId)
        {
            LuaLibrary = luaLibrary;
        }

        public override void Dispose()
        {
            LuaLibrary.ReferenceDropped(this);
        }
    }
}