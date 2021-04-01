using System.Collections.Generic;
using System.Text;

#nullable enable

namespace Slipstream.Components.Internal.Services
{
    internal class LuaService : ILuaSevice
    {
        private readonly List<ILuaGlue> LuaGlues = new List<ILuaGlue>();
        private readonly NLua.Lua Lua;

        public LuaService(
            IEnumerable<ILuaGlue>? luaGlues = null)
        {
            Lua = new NLua.Lua();
            Lua.State.Encoding = Encoding.UTF8;

            luaGlues ??= new List<ILuaGlue>();

            foreach (var lg in luaGlues)
            {
                LuaGlues.Add(lg);
            }

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