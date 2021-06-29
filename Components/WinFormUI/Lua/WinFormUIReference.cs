using Slipstream.Shared.Lua;

namespace Slipstream.Components.WinFormUI.Lua
{
    public class WinFormUIReference : BaseLuaReference, IWinFormUIReference
    {
        public WinFormUIReference(string instanceId, string luaScriptInstanceId) : base(instanceId, luaScriptInstanceId)
        {
        }
    }
}