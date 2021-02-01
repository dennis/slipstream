using System.Collections.Generic;

#nullable enable

namespace Slipstream.Components.Internal.Services
{
    internal class LuaService : ILuaSevice
    {
        private readonly List<ILuaGlue> LuaGlues;
        private readonly NLua.Lua Lua;

        public LuaService(
            List<ILuaGlue>? luaGlues = null)
        {
            Lua = new NLua.Lua();

            luaGlues ??= new List<ILuaGlue>();
            LuaGlues = luaGlues;

            foreach (var glue in luaGlues)
            {
                glue.SetupLua(Lua);
            }
        }

        public ILuaContext Parse(string filename)
        {
            return new LuaContext(filename, Lua);
        }

        public void Loop()
        {
            foreach (var glue in LuaGlues)
            {
                glue.Loop();
            }
        }
    }
}