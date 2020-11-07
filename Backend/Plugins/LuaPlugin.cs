using NLua;
using Slipstream.Shared;
using Slipstream.Shared.Events.Internal;
using System.Diagnostics;
using System.IO;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    class LuaPlugin : Worker, IPlugin, IEventListener
    {
        public System.Guid Id { get; set; }
        public string Name => "LuaPlugin";
        public string DisplayName { get; }
        public bool Enabled { get; internal set; }

        private IEventBusSubscription? Subscription;
        private readonly string FilePath;
        private readonly IEventBus EventBus;

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
        }

        protected override void Main()
        {
            LuaApi api
                = new LuaApi(EventBus, Path.GetFileName(FilePath));

            try
            {
                using var lua = new Lua();
                LuaFunction? handleFunc;

                lua.RegisterFunction("print", api, typeof(LuaApi).GetMethod("Print", new[] { typeof(string) }));
                var f = lua.LoadFile(FilePath);

                f.Call();

                handleFunc = lua["handle"] as LuaFunction;

                EventHandler eventHandler = new EventHandler();
                // Avoid that WriteToConsole is evaluated by Lua, that in turn will 
                // add more WriteToConsole events, making a endless loop
                eventHandler.OnUtilityWriteToConsole += (s, e) => {};
                eventHandler.OnDefault += (s, e) => handleFunc?.Call(e.Event);

                while (!Stopped)
                {
                    var e = Subscription?.NextEvent(250);

                    if (Enabled)
                        eventHandler.HandleEvent(e);
                }
            }
            catch (NLua.Exceptions.LuaScriptException e)
            {
                api.Print($"ERROR: {e.Message}");
                EventBus.PublishEvent(new Shared.Events.Internal.PluginUnregister() { Id = this.Id });
            }
        }
    }
}
