using NLua;
using Slipstream.Shared;
using Slipstream.Shared.Events.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using EventHandler = Slipstream.Shared.EventHandler;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    class LuaPlugin : IPlugin
    {
        public string Id { get; set; }
        public string Name => "LuaPlugin";
        public string DisplayName { get; }
        public bool Enabled { get; internal set; }
        public string WorkerName => "Lua";

        private IEventBusSubscription? Subscription;
        private readonly string FilePath;
        private readonly IEventBus EventBus;

        private readonly Lua Lua = new Lua();
        private readonly LuaFunction? HandleFunc;
        private readonly EventHandler EventHandler = new EventHandler();
        private readonly LuaApi Api;

        public LuaPlugin(string id, IEvent settings, IEventBus eventBus)
        {
            Id = id;
            if (!(settings is LuaSettings typedSettings))
                throw new System.Exception($"Unexpected event as Exception {settings}");
            if (typedSettings == null)
                throw new System.Exception($"Unexpected settings was null");

#pragma warning disable CS8601 // Possible null reference assignment.
            FilePath = typedSettings.FilePath;
#pragma warning restore CS8601 // Possible null reference assignment.
            DisplayName = $"Lua: {Path.GetFileName(FilePath)}";
            EventBus = eventBus;

            Api = new LuaApi(EventBus, Path.GetFileName(FilePath), Path.GetDirectoryName(FilePath));

            try
            {
                Lua.RegisterFunction("print", Api, typeof(LuaApi).GetMethod("Print", new[] { typeof(string) }));
                Lua.RegisterFunction("say", Api, typeof(LuaApi).GetMethod("Say", new[] { typeof(string), typeof(float) }));
                Lua.RegisterFunction("play", Api, typeof(LuaApi).GetMethod("Play", new[] { typeof(string), typeof(float) }));
                Lua.RegisterFunction("debounce", Api, typeof(LuaApi).GetMethod("Debounce", new[] { typeof(string), typeof(LuaFunction), typeof(float) }));
                Lua.RegisterFunction("write", Api, typeof(LuaApi).GetMethod("Write", new[] { typeof(string), typeof(string) }));

                var f = Lua.LoadFile(FilePath);

                f.Call();

                HandleFunc = Lua["handle"] as LuaFunction;

                // Avoid that WriteToConsole is evaluated by Lua, that in turn will 
                // add more WriteToConsole events, making a endless loop
                EventHandler.OnUtilityWriteToConsole += (s, e) => { };
                EventHandler.OnDefault += (s, e) => HandleFunc?.Call(e.Event);
            }
            catch (NLua.Exceptions.LuaScriptException e)
            {
                Api.Print($"ERROR: {e.Message}");
                EventBus.PublishEvent(new Shared.Events.Internal.PluginUnregister() { Id = this.Id });
            }
        }

        public void Disable(IEngine engine)
        {
            Enabled = false;
            Subscription?.Dispose();
            Subscription = null;
        }

        public void Enable(IEngine engine)
        {
            Enabled = true;
            Subscription = engine.RegisterListener();
        }

        public void RegisterPlugin(IEngine engine)
        {
        }

        public void UnregisterPlugin(IEngine engine)
        {
        }

        class LuaApi
        {
            private readonly IEventBus EventBus;
            private readonly string Prefix;
            private readonly IDictionary<string, DebounceInfo> DebouncedFunctions = new Dictionary<string, DebounceInfo>();
            private readonly string WorkDirectory;

            private class DebounceInfo
            {
                public LuaFunction Function;
                public DateTime TriggersAt;

                public DebounceInfo(LuaFunction luaFunction, DateTime triggersAt)
                {
                    Function = luaFunction;
                    TriggersAt = triggersAt;
                }
            }

            public LuaApi(IEventBus eventBus, string prefix, string workDirectory)
            {
                EventBus = eventBus;
                Prefix = prefix;
                WorkDirectory = workDirectory;
            }

            public void Print(string s)
            {
                EventBus.PublishEvent(new Shared.Events.Utility.WriteToConsole() { Message = $"{Prefix}: {s}" });
            }

            public void Say(string message, float volume)
            {
                EventBus.PublishEvent(new Shared.Events.Utility.Say() { Message = message, Volume = volume });
            }

            public void Play(string filename, float volume)
            {
                EventBus.PublishEvent(new Shared.Events.Utility.PlayAudio() { Filename = filename, Volume = volume });
            }

            public void Debounce(string name, LuaFunction func, float debounceLength)
            {
                Debug.WriteLine($"Debounce!  {func.GetHashCode()}");
                DebouncedFunctions[name] = new DebounceInfo(func, DateTime.Now.AddSeconds(debounceLength));
            }

            public void Write(string filePath, string content)
            {
                string fileDirectory = Path.GetDirectoryName(filePath);

                if(fileDirectory.Length == 0)
                {
                    filePath = WorkDirectory + @"\" + filePath;
                }

                using StreamWriter sw = File.AppendText(filePath);
                sw.WriteLine(content);
            }

            public void Loop()
            {
                string? deleteKey = null;

                foreach(var d in DebouncedFunctions)
                {
                    if(d.Value.TriggersAt < DateTime.Now)
                    {
                        d.Value.Function.Call();
                        deleteKey = d.Key;
                        break;
                    }
                }

                if (deleteKey != null)
                {
                    DebouncedFunctions.Remove(deleteKey);
                }
            }
        }

        public void Loop()
        {
            try
            {
                var e = Subscription?.NextEvent(250);

                if (Enabled)
                    EventHandler.HandleEvent(e);

                Api.Loop();
            }
            catch (NLua.Exceptions.LuaScriptException e)
            {
                Api.Print($"ERROR: {e.Message}");
                EventBus.PublishEvent(new Shared.Events.Internal.PluginUnregister() { Id = this.Id });
            }
        }
    }
}
