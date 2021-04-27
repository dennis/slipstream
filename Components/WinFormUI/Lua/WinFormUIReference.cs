namespace Slipstream.Components.WinFormUI.Lua
{
    public class WinFormUIReference : IWinFormUIReference
    {
        public WinFormUILuaLibrary LuaLibrary { get; }
        public string InstanceId { get; }

        public WinFormUIReference(string instanceId, WinFormUILuaLibrary luaLibrary)
        {
            LuaLibrary = luaLibrary;
            InstanceId = instanceId;
        }

        public void Dispose()
        {
            LuaLibrary.ReferenceDropped(this);
        }
    }
}