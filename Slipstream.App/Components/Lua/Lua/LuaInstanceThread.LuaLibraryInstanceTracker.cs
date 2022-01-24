#nullable enable

using NLua;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.Lua.Lua
{
    public partial class LuaInstanceThread
    {
        public class LuaLibraryInstanceTracker 
        {
            private readonly ILuaLibrary LuaLibrary;
            private readonly string LuaScriptInstanceId;
            private readonly LuaInstanceThread Controller;

            public LuaLibraryInstanceTracker(ILuaLibrary luaLibrary, LuaInstanceThread controller, string luaScriptInstanceId)
            {
                Controller = controller;
                LuaLibrary = luaLibrary;
                LuaScriptInstanceId = luaScriptInstanceId;
            }

            public string Name => LuaLibrary.Name;

            public void Dispose()
            {
                LuaLibrary.Dispose();
            }

#pragma warning disable IDE1006 // Naming Styles
            public ILuaReference? instance(LuaTable cfg)
            {
                var inst = LuaLibrary.GetInstance(LuaScriptInstanceId, cfg);
                if (inst != null)
                {
                    Controller.ReferenceCreated(inst);
                }
                return inst;
            }
#pragma warning restore IDE1006 // Naming Styles
        }
    }
}