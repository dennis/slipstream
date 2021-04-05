#nullable enable

namespace Slipstream.Components.Internal.LuaGlues
{
    public class StateLuaGlue : ILuaGlue
    {
        private readonly IStateService StateService;

        public StateLuaGlue(IStateService stateService)
        {
            StateService = stateService;
        }

        public void SetupLua(NLua.Lua lua)
        {
            lua["state"] = this;
            lua.DoString(@"
function get_state(a); return state:get(a); end
function set_state(a,b); return state:set(a, b); end
function set_temp_state(a,b,c); return state:set_temp(a,b,c); end
");
        }

        public void Loop()
        {
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