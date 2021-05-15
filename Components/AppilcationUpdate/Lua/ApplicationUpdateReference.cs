namespace Slipstream.Components.AppilcationUpdate.Lua
{
    public class ApplicationUpdateReference : IApplicationUpdateReference
    {
        private ApplicationUpdateLuaLibrary LuaLibrary { get; }
        public string InstanceId { get; }

        public ApplicationUpdateReference(ApplicationUpdateLuaLibrary luaLibrary, string instanceId)
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