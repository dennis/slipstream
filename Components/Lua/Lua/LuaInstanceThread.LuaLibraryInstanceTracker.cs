#nullable enable

using NLua;
using Slipstream.Shared.Lua;

namespace Slipstream.Components.Lua.Lua
{
    public partial class LuaInstanceThread
    {
        private class LuaLibraryInstanceTracker : ILuaLibrary
        {
            private readonly ILuaLibrary LuaLibrary;
            private readonly LuaInstanceThread Controller;

            public LuaLibraryInstanceTracker(ILuaLibrary luaLibrary, LuaInstanceThread controller)
            {
                Controller = controller;
                LuaLibrary = luaLibrary;
            }

            public string Name => LuaLibrary.Name;

            public void Dispose()
            {
                LuaLibrary.Dispose();
            }

            public ILuaReference? instance(LuaTable cfg)
            {
                var inst = LuaLibrary.instance(cfg);
                if (inst != null)
                {
                    Controller.ReferenceCreated(inst);
                }
                return inst;
            }
        }
    }
}