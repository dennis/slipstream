using NLua;
using Slipstream.Shared;
using Slipstream.Shared.Events.Internal;
using System.IO;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    class LuaPlugin : Worker, IPlugin
    {
        public System.Guid Id { get; set; }
        public string Name => "LuaPlugin";
        public string DisplayName { get; }
        public bool Enabled { get; internal set; }

        private IEventBusSubscription? Subscription;
        private readonly string FilePath;
        private readonly IEventBus EventBus;

        private readonly Lua Lua = new Lua();
        private readonly LuaFunction? HandleFunc;
        private readonly EventHandler EventHandler = new EventHandler();
        private readonly LuaApi Api;


        public LuaPlugin(IEvent settings, IEventBus eventBus)
        {
            if (!(settings is LuaSettings typedSettings))
                throw new System.Exception($"Unexpected event as Exception {settings}");
            if(typedSettings == null)
                throw new System.Exception($"Unexpected settings was null");

#pragma warning disable CS8601 // Possible null reference assignment.
            FilePath = typedSettings.FilePath;
#pragma warning restore CS8601 // Possible null reference assignment.
            Id = System.Guid.NewGuid();
            DisplayName = $"Lua: {Path.GetFileName(FilePath)}";
            EventBus = eventBus;

            Api = new LuaApi(EventBus, Path.GetFileName(FilePath));

            try
            {
                Lua.RegisterFunction("print", Api, typeof(LuaApi).GetMethod("Print", new[] { typeof(string) }));
                Lua.RegisterFunction("say", Api, typeof(LuaApi).GetMethod("Say", new[] { typeof(string), typeof(float) }));
                Lua.RegisterFunction("play", Api, typeof(LuaApi).GetMethod("Play", new[] { typeof(string), typeof(float) }));

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
            Start();
        }

        public void UnregisterPlugin(IEngine engine)
        {
            Stop();
        }

        class LuaApi
        {
            private readonly IEventBus EventBus;
            private readonly string Prefix;

            public LuaApi(IEventBus eventBus, string prefix)
            {
                EventBus = eventBus;
                Prefix = prefix;
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
        }


        override protected void Loop()
        {
            try
            {
                var e = Subscription?.NextEvent(250);

                if (Enabled)
                    EventHandler.HandleEvent(e);
            }
            catch (NLua.Exceptions.LuaScriptException e)
            {
                Api.Print($"ERROR: {e.Message}");
                EventBus.PublishEvent(new Shared.Events.Internal.PluginUnregister() { Id = this.Id });
            }
        }
    }
}
