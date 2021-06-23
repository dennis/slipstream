using Serilog;
using Slipstream.Components.Internal;
using Slipstream.Components.Lua.Lua;
using Slipstream.Shared;
using Slipstream.Shared.Lua;
using System;
using System.IO;

#nullable enable

namespace Slipstream.Backend
{
    internal class Engine : IEngine, IDisposable
    {
        private readonly IEventBus EventBus;
        private readonly ILogger Logger;
        private readonly ILuaLuaLibrary? LuaLuaLibrary;
        private readonly ILuaLuaReference? InitLuaScript;

        public Engine(
            ILogger logger,
            IEventBus eventBus,
            ILuaLibraryRepository luaLibraryRepository
        )
        {
            EventBus = eventBus;
            Logger = logger;
            LuaLuaLibrary = luaLibraryRepository.Get("api/lua") as LuaLuaLibrary;

            // init.lua..
            {
                const string initFilename = "init.lua";

                if (!File.Exists(initFilename))
                {
                    Logger.Information("No {initcfg} file found, creating", initFilename);
                    CreateInitLua(initFilename);
                }

                Logger.Information("Loading {initcfg}", initFilename);
                InitLuaScript = LuaLuaLibrary?.LoadLuaFile("init.lua", "init.lua", "init.lua") as ILuaLuaReference;
                InitLuaScript?.start();
            }

            // We're live - this will make eventbus distribute events
            EventBus.Enabled = true;
        }

        private void CreateInitLua(string initFilename)
        {
            var assembly = this.GetType().Assembly;
            using var initLuaStream = assembly.GetManifestResourceStream("Slipstream.Backend.Bootstrap.init.lua");
            using var sr = new StreamReader(initLuaStream);
            var initLuaContent = sr.ReadToEnd();

            File.WriteAllText(initFilename, initLuaContent);
        }

        public void Start()
        {
            InitLuaScript?.join();
        }

        public void Dispose()
        {
        }

        public void UnregisterSubscription(IEventBusSubscription subscription)
        {
        }
    }
}