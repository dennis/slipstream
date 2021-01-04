#nullable enable

using System;

namespace Slipstream.Backend
{
    public interface IPluginManager : IDisposable
    {
        public void UnregisterPlugin(IPlugin p);
        public void UnregisterPlugin(string id);
        public void RegisterPlugin(IPlugin plugin);
        public void EnablePlugin(IPlugin p);
        public void DisablePlugins();
        public void DisablePlugin(IPlugin p);
        public void FindPluginAndExecute(string pluginId, Action<IPlugin> a);
        public void ForAllPluginsExecute(Action<IPlugin> a);
        public void RestartReconfigurablePlugins();
    }
}