#nullable enable

using NLua;
using Slipstream.Components.Internal;
using Slipstream.Shared;

namespace Slipstream.Components.Lua.Lua
{
    public class StateLuaLibrary : ILuaLibrary
    {
        private readonly IStateService StateService;
        public string Name => "api/state";

        public StateLuaLibrary(IStateService stateService)
        {
            StateService = stateService;
        }

        public void Dispose()
        {
        }

        public ILuaReference? instance(LuaTable cfg)
        {
            return null;
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