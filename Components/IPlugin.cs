using Slipstream.Shared;
using System;

#nullable enable

namespace Slipstream.Components
{
    public interface IPlugin : IDisposable
    {
        public event EventHandler<IPlugin>? OnStateChanged;

        public string Id { get; }
        public string Name { get; }
        public string DisplayName { get; }
        public IEventHandlerController EventHandlerController { get; }
        public bool Reconfigurable { get; }
        public bool FullThreadControl { get; }

        public void Run();
    }
}