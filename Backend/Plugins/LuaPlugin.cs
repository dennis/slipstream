using Slipstream.Shared;
using Slipstream.Shared.Events.Internal;
using System.IO;
using System.Threading;

#nullable enable

namespace Slipstream.Backend.Plugins
{
    class LuaPlugin : Worker, IPlugin, IEventListener
    {
        public System.Guid Id { get; set; }
        public string Name => "LuaPlugin";
        public string DisplayName { get; }
        public bool Enabled { get; internal set; }

        public LuaPlugin(IEvent settings)
        {
            Id = System.Guid.NewGuid();

            if (!(settings is LuaSettings typedSettings))
                throw new System.Exception($"Unexpected event as Exception {settings}");

            DisplayName = $"Lua: {Path.GetFileName(typedSettings.FilePath)}";
        }

        public void Disable(IEngine engine)
        {
            Enabled = false;
        }

        public void Enable(IEngine engine)
        {
            Enabled = true;
        }

        public void RegisterPlugin(IEngine engine)
        {
            Start();
        }

        public void UnregisterPlugin(IEngine engine)
        {
            Stop();
        }

        protected override void Main()
        {
            while (!Stopped)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
