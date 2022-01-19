#nullable enable

namespace Slipstream.Shared.Lua
{
    public abstract class BaseLuaReference : ILuaReference
    {
        protected readonly IEventEnvelope Envelope;

        public string InstanceId { get; }
        public string LuaScriptInstanceId { get; }

        public BaseLuaReference(string instanceId, string luaScriptInstanceId)
        {
            InstanceId = instanceId;
            LuaScriptInstanceId = luaScriptInstanceId;
            Envelope = new EventEnvelope(luaScriptInstanceId).Add(instanceId);
        }
    }
}