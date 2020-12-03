using Slipstream.Shared;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Slipstream.Backend
{
    class PluginWorker : Worker
    {
        private readonly IList<IPlugin> Plugins = new List<IPlugin>();
        private readonly IEventBusSubscription Subscription;

        public PluginWorker(string name, IEventBusSubscription subscription) : base(name)
        {
            Subscription = subscription;
        }

        public void AddPlugin(IPlugin plugin)
        {
            lock (Plugins)
            {
                Plugins.Add(plugin);
                Debug.WriteLine($"[PluginWorker-{Name}] Adding plugin {plugin.Name} {plugin.Id}. Total of {Plugins.Count} plugins");
                foreach (var p in Plugins)
                {
                    Debug.WriteLine($" - {p.Id} {p.Name} {p.DisplayName}");
                }
            }
        }

        public void RemovePlugin(IPlugin plugin)
        {
            lock (Plugins)
            {
                Plugins.Remove(plugin);
                Debug.WriteLine($"[PluginWorker-{Name}] Remove plugin  {plugin.Name} {plugin.Id}. Total of {Plugins.Count} plugins");
                foreach (var p in Plugins)
                {
                    Debug.WriteLine($" - {p.Id} {p.Name} {p.DisplayName}");
                }
            }
        }

        override protected void Main()
        {
            while (!Stopped)
            {
                int invoked = 0;

                IEvent e;

                while ((e = Subscription.NextEvent(100)) != null)
                {
                    lock (Plugins)
                    {
                        foreach (var plugin in Plugins)
                        {
                            plugin.EventHandler.HandleEvent(e);
                        }
                    }
                }


                lock (Plugins)
                {
                    foreach (var plugin in Plugins)
                    {
                        plugin.Loop();
                        invoked += 1;
                    }
                }

                if (invoked == 0)
                {
                    // Avoid busy-looping if there are no plugins
                    Thread.Sleep(50);
                }
            }
        }
    }
}
