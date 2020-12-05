using NLua;
using Slipstream.Shared;
using Slipstream.Shared.Events.Internal;
using System;
using System.Collections.Generic;
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
        public EventHandler EventHandler { get; } = new EventHandler();

        private readonly string FilePath;
        private readonly IEventBus EventBus;

        private readonly Lua Lua = new Lua();
        private readonly LuaFunction? HandleFunc;
        private readonly LuaApi Api;

        public LuaPlugin(string id, IEvent settings, IEventBus eventBus)
        {
            Id = id;
            if (!(settings is LuaSettings typedSettings))
                throw new System.Exception($"Unexpected event as Exception {settings}");
            if (typedSettings == null)
                throw new System.Exception($"Unexpected settings was null");

            FilePath = typedSettings.FilePath;
            DisplayName = $"Lua: {Path.GetFileName(FilePath)}";
            EventBus = eventBus;

            Api = new LuaApi(EventBus, Path.GetFileName(FilePath), Path.GetDirectoryName(FilePath));

            try
            {
                Lua.RegisterFunction("print", Api, typeof(LuaApi).GetMethod("Print", new[] { typeof(string) }));
                Lua.RegisterFunction("say", Api, typeof(LuaApi).GetMethod("Say", new[] { typeof(string), typeof(float) }));
                Lua.RegisterFunction("play", Api, typeof(LuaApi).GetMethod("Play", new[] { typeof(string), typeof(float) }));
                Lua.RegisterFunction("debounce", Api, typeof(LuaApi).GetMethod("Debounce", new[] { typeof(string), typeof(LuaFunction), typeof(float) }));
                Lua.RegisterFunction("wait", Api, typeof(LuaApi).GetMethod("Wait", new[] { typeof(string), typeof(LuaFunction), typeof(float) }));
                Lua.RegisterFunction("write", Api, typeof(LuaApi).GetMethod("Write", new[] { typeof(string), typeof(string) }));
                Lua.RegisterFunction("send_twitch_message", Api, typeof(LuaApi).GetMethod("SendTwitchMessage", new[] { typeof(string) }));
                

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
        }

        public void Enable(IEngine engine)
        {
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
            private readonly IDictionary<string, DelayedExecution> DebouncedFunctions = new Dictionary<string, DelayedExecution>();
            private readonly IDictionary<string, DelayedExecution> WaitingFunctions = new Dictionary<string, DelayedExecution>();
            private readonly string WorkDirectory;

            private class DelayedExecution
            {
                public LuaFunction Function;
                public DateTime TriggersAt;

                public DelayedExecution(LuaFunction luaFunction, DateTime triggersAt)
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

            public void SendTwitchMessage(string message)
            {
                EventBus.PublishEvent(new Shared.Events.Twitch.TwitchSendMessage() { Message = message });
            }

            public void Debounce(string name, LuaFunction func, float debounceLength)
            {
                if(func != null)
                {
                    DebouncedFunctions[name] = new DelayedExecution(func, DateTime.Now.AddSeconds(debounceLength));
                }
                else
                {
                    Print("Can't debounce without a function");
                }
            }

            public void Wait(string name, LuaFunction func, float duration)
            {
                if(func != null)
                {
                    if (!WaitingFunctions.ContainsKey(name))
                        WaitingFunctions[name] = new DelayedExecution(func, DateTime.Now.AddSeconds(duration));
                }
                else
                {
                    Print("Can't wait without a function");
                }
            }

            public void Write(string filePath, string content)
            {
                string fileDirectory = Path.GetDirectoryName(filePath);

                if(fileDirectory.Length == 0)
                {
                    filePath = WorkDirectory + @"\" + filePath;
                }

                using StreamWriter sw = File.AppendText(filePath);
                sw.Write(content);
            }

            private  void HandleDelayedExecution(IDictionary<string, DelayedExecution> functions)
            {
                string? deleteKey = null;

                foreach (var d in functions)
                {
                    if (d.Value.TriggersAt < DateTime.Now)
                    {
                        d.Value.Function.Call();
                        deleteKey = d.Key;
                        break;
                    }
                }

                if (deleteKey != null)
                {
                    functions.Remove(deleteKey);
                }
            }

            public void Loop()
            {
                HandleDelayedExecution(DebouncedFunctions);
                HandleDelayedExecution(WaitingFunctions);
            }
        }

        public void Loop()
        {
            try
            {
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
