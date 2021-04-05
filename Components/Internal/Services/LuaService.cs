using Slipstream.Backend;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

#nullable enable

namespace Slipstream.Components.Internal.Services
{
    internal class LuaService : ILuaService
    {
        private readonly IPluginManager PluginManager;
        private readonly List<ILuaGlue> LuaGlues = new List<ILuaGlue>();

        public LuaService(IPluginManager pluginManager)
        {
            PluginManager = pluginManager;
        }

        public ILuaContext Parse(string filename)
        {
            if (LuaGlues.Count > 0)
                throw new System.Exception("This have already been populated");

            var Lua = new NLua.Lua();
            Lua.State.Encoding = Encoding.UTF8;

            PluginManager.ForAllPluginsExecute(plugin =>
            {
                foreach (var glue in plugin.CreateLuaGlues())
                {
                    LuaGlues.Add(glue);
                }
            });

            foreach (var glue in LuaGlues)
            {
                Debug.WriteLine($"Initializing NLua for {filename} with {glue}");
                glue.SetupLua(Lua);
            }

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