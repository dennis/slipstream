#nullable enable

using Slipstream.Components.Internal;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.Lua.Lua
{
    public class StateLuaReference : ILuaReference
    {
        private readonly IStateService StateService;
        private readonly StateLuaLibrary LuaLibrary;
        public string InstanceId { get; }

        public StateLuaReference(StateLuaLibrary luaLibrary, string instanceId, IStateService stateService)
        {
            LuaLibrary = luaLibrary;
            InstanceId = instanceId;
            StateService = stateService;
        }

        public void Dispose()
        {
            LuaLibrary.ReferenceDropped(this);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public string get(string key)
        {
            return StateService.GetState(key);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void set(string key, string value)
        {
            StateService.SetState(key, value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "This is expose in Lua, so we want to keep that naming style")]
        public void set_temp(string key, string value, int lifetimeInSeconds)
        {
            StateService.SetState(key, value, lifetimeInSeconds);
        }
    }
}